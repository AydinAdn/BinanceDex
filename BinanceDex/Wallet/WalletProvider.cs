using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using BinanceDex.Utilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BinanceDex.Wallet
{
    public interface IProcessManager : IDisposable
    {
        void Start(string argument);
        void Send(string argument);
        DataReceivedEventHandler DataReceivedEventHandler { get; set; }
    }

    public interface IProcessManagerFactory
    {
        IProcessManager CreateProcessManager();
    }

    public class ProcessManagerFactory : IProcessManagerFactory
    {
        private readonly string cliPath;
        private readonly int timeOutInMs;
        public ProcessManagerFactory(string cliPath, int timeOutInMs = 3000)
        {
            this.cliPath = cliPath;
            this.timeOutInMs = timeOutInMs;
        }

        public IProcessManager CreateProcessManager()
        {
            return new ProcessManager(this.cliPath, this.timeOutInMs);
        }
    }

    public class ProcessManager : IProcessManager
    {
        private bool isDisposed;
        private readonly string cliPath;
        private readonly Process process;
        private readonly int timeOutInMs;

        public ProcessManager(string bnbcliPath, int timeOutInMs=3000)
        {
            this.cliPath = bnbcliPath;
            this.timeOutInMs = timeOutInMs;
            this.process = new Process();
        }

        public void Start(string argument)
        {
            Throw.IfNullOrWhiteSpace(argument, nameof(argument));

            this.process.StartInfo = this.GenerateStartInfo(argument);
            this.process.OutputDataReceived += this.DataReceivedEventHandler;
            this.process.ErrorDataReceived += this.DataReceivedEventHandler;

            this.process.Start();
            this.process.BeginOutputReadLine();
            this.process.BeginErrorReadLine();
        }

        public void Send(string argument)
        {
            Throw.IfNullOrWhiteSpace(argument, nameof(argument));

            this.process.StandardInput.WriteLine(argument);
        }

        public DataReceivedEventHandler DataReceivedEventHandler { get; set; }

        private ProcessStartInfo GenerateStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = this.cliPath,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                Arguments = arguments,
                RedirectStandardInput = true,
            };
        }


        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        protected void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (this.isDisposed) return;

            this.process.WaitForExit(this.timeOutInMs);
            this.process?.Dispose();
            this.isDisposed = true;
        }
    }


    [SuppressMessage("ReSharper", "InvertIf")]
    public class WalletProvider : IWalletProvider
    {
        private readonly ILogger<IWalletProvider> logger;
        private readonly IProcessManagerFactory processManagerFactory;

        public WalletProvider(IProcessManagerFactory processManagerFactory, ILogger<IWalletProvider> logger)
        {
            this.logger = logger;
            this.processManagerFactory = processManagerFactory;
        }

        public WalletProvider(string bnbCliPath, ILogger<IWalletProvider> logger) : this(new ProcessManagerFactory(bnbCliPath), logger)
        {
        }

        #region Async

        public Task<IWalletProviderResult<WalletInfo>> FindWalletAsync(string walletName)
        {
            return Task.Run(() => this.FindWallet(walletName));
        }

        public Task<IWalletProviderResult<WalletInfo>> CreateWalletAsync(string walletName, string password, string confirmPassword, bool overwrite = false)
        {
            return Task.Run(() => this.CreateWallet(walletName, password, confirmPassword, overwrite));
        }

        public Task<IWalletProviderResult<WalletInfo>> ImportWalletAsync(string walletName, string password, string confirmPassword, string seedPhrase, bool overwrite = false)
        {
            return Task.Run(() => this.ImportWallet(walletName, password, confirmPassword, seedPhrase));
        }

        public Task<IWalletProviderResult> UpdateWalletAsync(string walletName, string currentPassword, string newPassword, string confirmPassword)
        {
            return Task.Run(() => this.UpdateWallet(walletName,currentPassword,newPassword,confirmPassword));
        }

        public Task<IWalletProviderResult> RemoveWalletAsync(string walletName, string password)
        {
            return Task.Run(()=> this.RemoveWallet(walletName, password));
        }

        public Task<IWalletProviderResult<IEnumerable<WalletInfo>>> GetAllWalletsAsync()
        {
            return Task.Run(() => this.GetAllWallets());
        }

        #endregion

        #region Non-Async

        public IWalletProviderResult<WalletInfo> CreateWallet(string walletName, string password, string confirmPassword, bool overwrite = false)
        {
            Throw.IfNullOrWhiteSpace(walletName, nameof(walletName));
            Throw.IfNullOrWhiteSpace(password, nameof(password));
            Throw.IfNullOrWhiteSpace(confirmPassword, nameof(confirmPassword));

            WalletInfo account = this.FindWallet(walletName).Result;

            if (account != null && overwrite == false) return new FailedWalletProviderResult<WalletInfo>($@"Wallet ""{walletName}"" already exists, to overwrite, set overwrite to true");
            if (password.Length < 8) return new FailedWalletProviderResult<WalletInfo>($"Password must be at least 8 characters");
            if (password != confirmPassword) return new FailedWalletProviderResult<WalletInfo>($"Password and confirmation password does not match");

            string response = string.Empty;

            using (IProcessManager process = this.processManagerFactory.CreateProcessManager())
            {
                process.DataReceivedEventHandler += (o, e) =>
                {
                    if (e.Data == null) return;
                    response = e.Data;
                };

                process.Start($@"keys add {walletName} -o json");

                if (overwrite) process.Send("y");

                process.Send(password);
                process.Send(confirmPassword);
            }

            if (response.StartsWith("ERROR"))
            {
                this.logger?.LogError($"{nameof(IWalletProvider)}.{nameof(this.CreateWallet)}: {response}");
                return new FailedWalletProviderResult<WalletInfo>(response);
            }

            return new SuccessfulWalletProviderResult<WalletInfo>(JsonConvert.DeserializeObject<WalletInfo>(response));
        }

        public IWalletProviderResult<WalletInfo> FindWallet(string walletName)
        {

            Throw.IfNullOrWhiteSpace(walletName, nameof(walletName));

            string response = string.Empty;
            using (IProcessManager process = this.processManagerFactory.CreateProcessManager())
            {
                process.DataReceivedEventHandler += (o, e) =>
                {
                    if (e.Data == null) return;
                    response = e.Data;
                };

                process.Start($@"keys show {walletName} -o json");
            }


            if (response.StartsWith("ERROR"))
            {
                this.logger?.LogError($"{nameof(IWalletProvider)}.{nameof(this.FindWallet)}: {response}");

                return new FailedWalletProviderResult<WalletInfo>(response);
            }

            return new SuccessfulWalletProviderResult<WalletInfo>(JsonConvert.DeserializeObject<WalletInfo>(response));
        }

        public IWalletProviderResult<IEnumerable<WalletInfo>> GetAllWallets()
        {
            string response = string.Empty;
            using (IProcessManager process = this.processManagerFactory.CreateProcessManager())
            {
                process.DataReceivedEventHandler += (o, e) =>
                {
                    if (e.Data == null) return;
                    response = e.Data;
                };                               

                process.Start($@"keys list -o json");
            }


            if (response.StartsWith("ERROR"))
            {
                this.logger?.LogError($"{nameof(IWalletProvider)}.{nameof(this.GetAllWallets)}: {response}");

                return new FailedWalletProviderResult<IEnumerable<WalletInfo>>(response);
            }

            return new SuccessfulWalletProviderResult<IEnumerable<WalletInfo>>(JsonConvert.DeserializeObject<IList<WalletInfo>>(response));
        }

        public IWalletProviderResult<WalletInfo> ImportWallet(string walletName, string password, string confirmPassword, string seedPhrase, bool overwrite = false)
        {
            Throw.IfNullOrWhiteSpace(walletName, nameof(walletName));
            Throw.IfNullOrWhiteSpace(password, nameof(password));
            Throw.IfNullOrWhiteSpace(confirmPassword, nameof(confirmPassword));
            Throw.IfNullOrWhiteSpace(seedPhrase, nameof(seedPhrase));

            WalletInfo account = this.FindWallet(walletName).Result;

            if (account != null && overwrite == false) return new FailedWalletProviderResult<WalletInfo>($@"Account ""{walletName}"" already exists, to overwrite, set overwrite to true");
            if (password.Length < 8) return new FailedWalletProviderResult<WalletInfo>($"Password must be at least 8 characters");
            if (password != confirmPassword) return new FailedWalletProviderResult<WalletInfo>($"Password and confirmation password does not match");

            string response = string.Empty;

            using (IProcessManager process = this.processManagerFactory.CreateProcessManager())
            {
                process.DataReceivedEventHandler += (o, e) =>
                {
                    if (e.Data == null) return;
                    response = e.Data;
                };

                process.Start($@"keys add {walletName} --recover -o json");
             

                if (overwrite)
                process.Send("y");
                process.Send(password);
                process.Send(seedPhrase);
            }


            if (response.StartsWith("ERROR"))
            {
                this.logger?.LogError($"{nameof(IWalletProvider)}.{nameof(this.ImportWallet)}: {response}");

                return new FailedWalletProviderResult<WalletInfo>(response);
            }
            return new SuccessfulWalletProviderResult<WalletInfo>(JsonConvert.DeserializeObject<WalletInfo>(response));
        }

        public IWalletProviderResult RemoveWallet(string walletName, string password)
        {
            Throw.IfNullOrWhiteSpace(walletName, nameof(walletName));
            Throw.IfNullOrWhiteSpace(password, nameof(password));

            var account = this.FindWallet(walletName).Result;

            if (account == null) return new FailedWalletProviderResult<WalletInfo>($@"Account ""{walletName}"" does not exist");
            if (password.Length < 8) return new FailedWalletProviderResult<WalletInfo>($"Password must be at least 8 characters");

            string response = string.Empty;

            using (IProcessManager process = this.processManagerFactory.CreateProcessManager())
            {
                process.DataReceivedEventHandler += (o, e) =>
                {
                    if (e.Data == null) return;
                    response = e.Data;
                };

                process.Start($@"keys delete {walletName} -o json");
               
                process.Send(password);
            }

            if (response.StartsWith("ERROR"))
            {
                this.logger?.LogError($"{nameof(IWalletProvider)}.{nameof(this.RemoveWallet)}: {response}");

                return new FailedWalletProviderResult<WalletInfo>(response);
            }

            return new SuccessfulWalletProviderResult();
        }

        public IWalletProviderResult UpdateWallet(string walletName, string currentPassword, string newPassword, string confirmPassword)
        {
            Throw.IfNullOrWhiteSpace(walletName, nameof(walletName));
            Throw.IfNullOrWhiteSpace(currentPassword, nameof(currentPassword));
            Throw.IfNullOrWhiteSpace(newPassword, nameof(newPassword));
            Throw.IfNullOrWhiteSpace(confirmPassword, nameof(confirmPassword));

            if (this.FindWallet(walletName).Result == null) return new FailedWalletProviderResult<WalletInfo>($@"Account ""{walletName}"" does not exist");
            if (currentPassword.Length < 8) return new FailedWalletProviderResult<WalletInfo>("Current password must be at least 8 characters");
            if (newPassword.Length < 8) return new FailedWalletProviderResult<WalletInfo>("New password must be at least 8 characters");
            if (newPassword != confirmPassword) return new FailedWalletProviderResult<WalletInfo>("New password and confirmation passwords do not match.");

            string response = string.Empty;

            using (IProcessManager process = this.processManagerFactory.CreateProcessManager())
            {
                process.DataReceivedEventHandler += (o, e) =>
                {
                    if (e.Data == null) return;
                    response = e.Data;
                };

                process.Start($@"keys update {walletName} -o json");
              

                process.Send(currentPassword);
                process.Send(newPassword);
                process.Send(confirmPassword);
            }

            if (response.StartsWith("ERROR"))
            {
                this.logger?.LogError($"{nameof(IWalletProvider)}.{nameof(this.UpdateWallet)}: {response}");

                return new FailedWalletProviderResult(response);
            }

            return new SuccessfulWalletProviderResult();
        }

        #endregion
    }
}