using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Nhb_Clientes.Models
{
    public class Nhb_Helper
    {

        public static NHibernate.ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                .ConnectionString(@"Data Source=Marcos_Afonso\SQLEXPRESS;Initial Catalog=Cadastro;Trusted_Connection=True;TrustServerCertificate=True")
                .ShowSql()
                )
                .Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<Clientes>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                    .Create(false, false))
                .BuildSessionFactory();

            return sessionFactory.OpenSession();
        }
    }
}
