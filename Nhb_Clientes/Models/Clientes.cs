namespace Nhb_Clientes.Models
{
    public class Clientes
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sexo { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string? Telefone1 { get; set; }
        public virtual string? Telefone2 { get; set; }
        public virtual string? Telefone3 { get; set; }
        public virtual int FoneValido { get; set; }
        public virtual string? Tel_Ativo { get; set; }
    }
}
