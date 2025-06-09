using FluentNHibernate.Mapping;

namespace Nhb_Clientes.Models
{
    class ClientesMap : ClassMap<Clientes>
    {
        public ClientesMap()
        {
            Id(x => x.Id);
            Map(x => x.Nome);
            Map(x => x.Sexo);
            Map(x => x.Endereco);
            Map(x => x.Telefone1);
            Map(x => x.Telefone2);
            Map(x => x.Telefone3);
            Map(x => x.FoneValido);
            Map(x => x.Tel_Ativo);
            Table("Clientes");
        }
    }
}
