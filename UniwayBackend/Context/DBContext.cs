using Microsoft.EntityFrameworkCore;
using UniwayBackend.Models.Entities;

namespace UniwayBackend.Context
{
    public class DBContext : DbContext
    {

        // Propiedades DbSet para definir las entidades
        // DbSet => Para consultar y guardar instancias. Permitiendo usar Query LINQ
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Technical> technicals { get; set; }
        public DbSet<UserTechnical> UserTechnicals { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Experience> Experiences{ get; set; }
        public DbSet<Profession> Professions{ get; set; }
        public DbSet<WorkshopTechnicalProfession> WorkshopTechnicalProfessions { get; set; }


        // Constructor de la clase DBContext que recibe opciones de configuración de DbContext
        public DBContext() { }


        // OnConfiguring => se utiliza para configurar las opciones del contexto de la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Compruebamos si ya se han configurado las opciones (puede que ya estén configuradas en el constructor)
            if (!optionsBuilder.IsConfigured)
            {
                // Accede a la configuración de la aplicación
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.Development.json")
                    .Build();

                // Obtiene la cadena de conexión desde appsettings.json usando el método GetConnectionString
                var connectionString = config.GetConnectionString("connUniway");

                // Configura las opciones del DbContext con la cadena de conexión obtenida
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        // OnModelCreating => se utiliza para configurar el modelo de datos de la aplicación.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configuración de la entidad Role
            modelBuilder.Entity<Role>(role =>
            {
                role.HasKey(r => r.Id);
                role.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(50); // Correspondencia con VARCHAR(50)
            });

            // Configuración de la entidad User
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.Id);
                user.Property(u => u.RoleId)
                    .HasColumnType("smallint")
                    .IsRequired();
                user.Property(u => u.Email)
                    .IsRequired();
                user.Property(u => u.Password)
                    .IsRequired();
                user.Property(u => u.Enabled)
                    .IsRequired();
                user.Property(u => u.CreatedOn);
                user.Property(u => u.UpdatedOn);

                // Definición de la relación con la entidad Role
                user.HasOne(u => u.Role)
                    .WithMany() // Un rol puede estar relacionado con varios usuarios
                    .HasForeignKey(u => u.RoleId);
            });

            // Configuración de la entidad Technical
            modelBuilder.Entity<Technical>(technical =>
            {
                technical.HasKey(t => t.Id);

                technical.HasIndex(t => t.Dni) // Declaramos que el DNI sea unico
                    .IsUnique();

                technical.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(45); // Correspondencia con VARCHAR(45)
                technical.Property(t => t.FatherLastname)
                    .IsRequired()
                    .HasMaxLength(45); // Correspondencia con VARCHAR(45)
                technical.Property(t => t.MotherLastname)
                    .IsRequired()
                    .HasMaxLength(45); // Correspondencia con VARCHAR(45)
                technical.Property(t => t.Dni) // Declaramos las restricciones del DNI como propiedad
                    .IsRequired()
                    .HasMaxLength(8); // Correspondencia con VARCHAR(8)
                technical.Property(t => t.BirthDate)
                    .IsRequired();
                technical.Property(t => t.Lat)
                    .HasColumnType("decimal(15, 10)"); // Correspondencia con DECIMAL(15,10)
                technical.Property(t => t.Lng)
                    .HasColumnType("decimal(15, 10)"); // Correspondencia con DECIMAL(15,10)
                technical.Property(t => t.Enabled)
                    .IsRequired();

            });


            modelBuilder.Entity<UserTechnical>(userT =>
            {
                userT.HasKey(ut => ut.Id);
                
                userT.Property(ut => ut.UserId)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();
                
                userT.Property(ut => ut.TechnicalId)
                    .IsRequired();

                userT.Property(ut => ut.Enabled)
                    .IsRequired();

                // Relations
                userT.HasOne(ut => ut.User)
                    .WithMany()
                    .HasForeignKey(userT => userT.UserId);

                userT.HasOne(ut => ut.Technical)
                    .WithMany()
                    .HasForeignKey(ut => ut.TechnicalId);
            });

            // Configuración de la entidad Client
            modelBuilder.Entity<Client>(client =>
            {
                client.HasKey(c => c.Id);
                client.Property(c => c.UserId)
                    .HasColumnType("uniqueidentifier")
                    .IsRequired();
                client.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(45); // Correspondencia con VARCHAR(45)
                client.Property(c => c.FatherLastname)
                    .IsRequired()
                    .HasMaxLength(45); // Correspondencia con VARCHAR(45)
                client.Property(c => c.MotherLastname)
                    .IsRequired()
                    .HasMaxLength(45); // Correspondencia con VARCHAR(45)
                client.Property(c => c.Dni)
                    .IsRequired()
                    .HasMaxLength(45); // Correspondencia con VARCHAR(45)
                client.Property(c => c.BirthDate)
                    .IsRequired();
                client.Property(c => c.Enabled)
                    .IsRequired();

                // Definición de la relación con la entidad User
                client.HasOne(c => c.User)
                    .WithMany() // Un usuario puede estar relacionado con varios clientes
                    .HasForeignKey(c => c.UserId);
            });

            // Availability
            modelBuilder.Entity<Availability>(availability =>
            {
                availability.HasKey(a => a.Id);
                availability.Property(a => a.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // Experience
            modelBuilder.Entity<Experience>(experience =>
            {
                experience.HasKey(a => a.Id);
                experience.Property(a => a.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // Profession
            modelBuilder.Entity<Profession>(profession =>
            {
                profession.HasKey(a => a.Id);
                profession.Property(a => a.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            });

            // TechnicalProfession
            modelBuilder.Entity<TechnicalProfession>(techProf =>
            {
                techProf.HasKey(tp => tp.Id);

                techProf.Property(tp => tp.ExperienceId)
                    .HasColumnType("smallint")
                    .IsRequired();

                techProf.Property(tp => tp.ProfessionId)
                    .HasColumnType("smallint")
                    .IsRequired();

                techProf.Property(tp => tp.UserTechnicalId)
                    .IsRequired();

                techProf.HasOne(tp => tp.UserTechnical)
                    .WithMany()
                    .HasForeignKey(tp => tp.UserTechnicalId);
            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
