using System;
using AutoMapper;
using BinanceDex.Api;
using BinanceDex.Api.Models;
using BinanceDex.Wallet;
using BinanceDex.WebSockets;
using BinanceDex.WebSockets.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace BinanceDex.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWalletManager(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<IProcessManagerFactory, ProcessManagerFactory>(p => new ProcessManagerFactory(configuration.GetSection("BinanceCli:File")["Path"], Convert.ToInt32(configuration.GetSection("BinanceCli:File")["Timeout"])));
            services.AddTransient<IWalletProvider, WalletProvider>(provider => new WalletProvider(provider.GetService<IProcessManagerFactory>(), provider.GetService<ILogger<IWalletProvider>>()));

            return services;
        }

        public static IServiceCollection AddDexApi(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<IHttp, Http>();
            services.AddTransient<IBinanceDexApi, BinanceDexApi>(provider => new BinanceDexApi(provider.GetService<IHttp>(), configuration.GetSection("BinanceApi")["BaseUrl"], configuration.GetSection("BinanceApi")["Hrp"]));

            return services;
        }
        public static IServiceCollection AddDexWebSockets(this IServiceCollection services, IConfigurationRoot configuration)
        {

            bool keepAlive = bool.Parse(configuration.GetSection("BinanceApi")["KeepWssAlive"]);
            services.AddTransient<ICandleStickWebSocket, CandleStickWebSocket>(provider => new CandleStickWebSocket(configuration.GetSection("BinanceApi")["BaseWssUrl"], keepAlive));

            return services;

        }
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {

            services.AddSingleton<IConfigurationProvider, MapperConfiguration>(p =>
            {
                return new MapperConfiguration(expression =>
                {
                    expression.CreateMap<KLine, CandleStick>();
                });
            });
            
            services.AddSingleton<IMapper, Mapper>(provider => new Mapper(provider.GetService<IConfigurationProvider>()));

            return services;

        }


    }
}