using System;
using Autofac;
using Br.Com.BiscoitinhosVovoLiva.Bootstrapper.LogModule;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.RepositorioJSON;
using Br.Com.BiscoitinhosVovoLiva.Servico.Implementacoes;
using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP;
using log4net;

namespace Br.Com.BiscoitinhosVovoLiva.Bootstrapper
{
    public class Bootstrapper
    {
        public static ContainerBuilder ObterContainer()
        {
            try
            {
                //Inicializar o container
                var builder = new ContainerBuilder();

                #region Entidades

                //Registrando as entidades.
                var entidades = typeof(Pedido).Assembly;
                builder.RegisterAssemblyTypes(entidades)
                    .Where(t => t.BaseType == typeof(Pedido))
                    .AsSelf();

                #endregion

                #region Repositorios

                // Registrando os repositórios implementados com JSON.
                var dataAccess = typeof(RepositorioJSON<>).Assembly;
                builder.RegisterAssemblyTypes(dataAccess)
                    .Where(t => t.Name.EndsWith("Repositorio"))
                    .AsImplementedInterfaces();

                #endregion

                #region Serviços

                // Registrando classes de serviço.
                var servicos = typeof(BaseService).Assembly;
                builder.RegisterAssemblyTypes(servicos)
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces()
                    .PropertiesAutowired();

                // Registrando serviço de integração com o AD.
                builder.RegisterType<ActiveDirectoryUtility>();

                #endregion

                #region Log4Net

                // Registra o modulo responsavel por resolver ILog nos construtores.
                builder.RegisterModule(new LoggingModule());
                // Registra a dependencia para a interface ILog. (Quando não resolvido pelo construtor).
                builder.RegisterInstance(LogManager.GetLogger("BDVLiva"))
                    .As<ILog>();

                #endregion

                return builder;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
