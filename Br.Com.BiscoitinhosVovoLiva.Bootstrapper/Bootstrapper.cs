using System;
using Autofac;
using Autofac.Integration.Mvc;
using Br.Com.BiscoitinhosVovoLiva.Bootstrapper.LogModule;
using Br.Com.BiscoitinhosVovoLiva.Entidade;
using Br.Com.BiscoitinhosVovoLiva.Entidade.Mapemanetos;
using Br.Com.BiscoitinhosVovoLiva.RepositorioJSON;
using Br.Com.BiscoitinhosVovoLiva.RepositorioSQLSERVER;
using Br.Com.BiscoitinhosVovoLiva.Servico.Implementacoes;
using Br.Com.BiscoitinhosVovoLiva.Servico.LDAP;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using log4net;
using NHibernate;
using NHibernate.Cfg;

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

                #region Fluent
                //Registra no container a sessão do Fluent de acordo com a conexão e definido o contexto da sessão.
                builder.Register(x => GetSessionFactory("thread_static", "ConexaoSQLSERVER"))
                    .SingleInstance();
                #endregion

                #region ISession
                builder.Register(x =>
                {
                    var session = x.Resolve<ISessionFactory>().OpenSession();
                    session.FlushMode = FlushMode.Commit;
                    return session;
                }).InstancePerHttpRequest().OnRelease(x => x.Dispose());
                #endregion

                #region Entidades

                //Registrando as entidades.
                var entidades = typeof(Pedido).Assembly;
                builder.RegisterAssemblyTypes(entidades)
                    .Where(t => t.BaseType == typeof(Pedido))
                    .AsSelf();

                #endregion

                #region Repositorios

                // Registrando os repositórios implementados com JSON.
                var repositorioJSON = typeof(RepositorioJSON<>).Assembly;
                builder.RegisterAssemblyTypes(repositorioJSON)
                    .Where(t => t.Name.EndsWith("Repositorio"))
                    .AsImplementedInterfaces();

                // Registrando os repositórios implementados para SQLSERVER.
                var repositorioSQLSERVER = typeof(RepositorioSQLSERVER<>).Assembly;
                builder.RegisterAssemblyTypes(repositorioSQLSERVER)
                    .Where(t => t.Name.EndsWith("Repositorio"))
                    .AsImplementedInterfaces()
                    .PropertiesAutowired();

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

        public static ISessionFactory GetSessionFactory(string currentSessionContextClass, string connectionStringKey)
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008.ShowSql()
                    .ConnectionString(c => c.FromConnectionStringWithKey(connectionStringKey)))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(PedidoMap).Assembly))
                    .ExposeConfiguration(x => x.SetProperty(NHibernate.Cfg.Environment.CurrentSessionContextClass, currentSessionContextClass))
                    .Cache(x => x.UseQueryCache())
                    .BuildSessionFactory();
        }
    }
}
