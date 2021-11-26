using Microsoft.EntityFrameworkCore;

namespace DemoWebServiceEntityFramework.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)  //DbContextOptions permite receber as configuracoes de acesso da string de conexao (recebe de program.cs)
        : base(options)
    {
    }
    public TodoContext()
    {
    }
    
    public DbSet<TodoItem> TodoItems { get; set; } = null!;  //colecao de entidades (tabela TodoItems)

    protected override void OnModelCreating(ModelBuilder modelBuilder) //permite ajustar o mapeamento objeto TodoItem para a estrutura da base de dados
    {
        modelBuilder.Entity<TodoItem>()
            .ToTable("Todos")    //nome da tabela
            .HasKey(t => t.Id)   //indicando a chave primaria
            .HasName("PK_Todos"); //nome da chave primaria no banco de dados

        modelBuilder.Entity<TodoItem>()
            .Property(t => t.Name)
            .HasMaxLength(200) //tamanho maximo 200
            .IsRequired();  //é nullable pro c#, mas eh obrigatoria pro banco de dados

        /*modelBuilder.Entity<TodoItem>(eb => {
            eb.ToTable("Todos")    //nome da tabela
            .HasKey(t => t.Id)   //indicando a chave 
            .HasName("PK_Todos"); //nome da chave primaria

            eb.Property(t => t.Name)
            .HasMaxLength(200) //tamanho maximo 200
            .IsRequired();  //é nullable pro c#, mas eh obrigatoria pro banco de dados
        });*/
    }
}