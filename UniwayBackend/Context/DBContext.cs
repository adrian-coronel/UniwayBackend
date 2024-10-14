using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Core.Response;

namespace UniwayBackend.Context
{
    public class DBContext : DbContext
    {

        // Propiedades DbSet para definir las entidades
        // DbSet => Para consultar y guardar instancias. Permitiendo usar Query LINQ
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Technical> Technicals { get; set; }
        public DbSet<UserTechnical> UserTechnicals { get; set; }
        public DbSet<Client> clients { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<WorkshopTechnicalProfession> WorkshopTechnicalProfessions { get; set; }
        public DbSet<TechnicalProfession> TechnicalProfessions { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<ServiceTechnical> ServiceTechnicals { get; set; }
        public DbSet<TechnicalProfessionAvailability> TechnicalProfessionAvailabilities { get; set; }
        public DbSet<TechnicalProfessionAvailabilityRequest> TechnicalProfessionAvailabilityRequests { get; set; }
        public DbSet<CategoryService> CategoryServices { get; set; }
        public DbSet<ImagesProblemRequest> ImagesProblemRequests { get; set; }
        public DbSet<CategoryRequest> CategoryRequests { get; set; }
        public DbSet<StateRequest> StateRequest { get; set; }
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<TechnicalResponse> TechnicalResponses { get; set; }
        public DbSet<Material> Materials { get; set; }
        public virtual DbSet<UserRequest> UserRequests { get; set; }
         

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
                optionsBuilder.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
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
                    .IsRequired()
                    .HasMaxLength(45);
                user.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                user.Property(u => u.Enabled)
                    .IsRequired();
                user.Property(u => u.CreatedOn);
                user.Property(u => u.UpdatedOn);

                // Definición de la relación con la entidad Role
                user.HasOne(u => u.Role)
                    .WithMany() // Un rol puede estar relacionado con varios usuarios
                    .HasForeignKey(u => u.RoleId);

                user.HasMany(u => u.UserTechnicals)
                    .WithOne(ut => ut.User)
                    .HasForeignKey(ut => ut.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
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
                technical.Property(t => t.Location)
                    .HasColumnType("geography");
                technical.Property(t => t.Enabled)
                    .IsRequired();

                // Technical - UserTechnical (One-to-Many)
                technical.HasMany(t => t.UserTechnicals)
                    .WithOne(ut => ut.Technical)
                    .HasForeignKey(ut => ut.TechnicalId)
                    .OnDelete(DeleteBehavior.Cascade); // Si lo deseas en cascada

                // Technical - Reviews (One-to-Many)
                technical.HasMany(t => t.Reviews)
                    .WithOne(r => r.Technical)
                    .HasForeignKey(r => r.TechnicalId)
                    .OnDelete(DeleteBehavior.Cascade);

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
                    .WithMany(u => u.UserTechnicals)
                    .HasForeignKey(userT => userT.UserId);


                // UserTechnical - TechnicalProfession (One-to-Many)
                userT.HasMany(ut => ut.TechnicalProfessions)
                    .WithOne(tp => tp.UserTechnical)
                    .HasForeignKey(tp => tp.UserTechnicalId)
                    .OnDelete(DeleteBehavior.Cascade);

                // UserTechnical - TowingCars (One-to-Many)
                userT.HasMany(ut => ut.TowingCars)
                    .WithOne(tc => tc.UserTechnical)
                    .HasForeignKey(tc => tc.UserTechnicalId)
                    .OnDelete(DeleteBehavior.Cascade);

                userT.HasOne(ut => ut.Technical)
                    .WithMany(t => t.UserTechnicals)
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
                    .WithOne()
                    .HasForeignKey<Client>(c => c.UserId);


                client.HasMany(c => c.Reviews)
                    .WithOne(r => r.Client)
                    .HasForeignKey(r => r.ClientId);
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
                    .WithMany(u => u.TechnicalProfessions) // Importante especificar la relación inversa si la hemos definido
                    .HasForeignKey(tp => tp.UserTechnicalId);


                // TechnicalProfession - Profession (Many-to-One)
                techProf.HasOne(tp => tp.Profession)
                    .WithMany()
                    .HasForeignKey(tp => tp.ProfessionId);

                // TechnicalProfession - Experience (Many-to-One)
                techProf.HasOne(tp => tp.Experience)
                    .WithMany()
                    .HasForeignKey(tp => tp.ExperienceId);

                // TechnicalProfession - TechnicalProfessionAvailability (One-to-Many)
                techProf.HasMany(tp => tp.TechnicalProfessionAvailabilities)
                    .WithOne(tpa => tpa.TechnicalProfession)
                    .HasForeignKey(tpa => tpa.TechnicalProfessionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Review - Request (Many-to-One)
            modelBuilder.Entity<Review>(review =>
            {
                review.HasKey(r => r.Id);

                review.HasOne(r => r.Request)
                .WithMany(r => r.Reviews)
                .HasForeignKey(r => r.RequestId);

                review.HasOne(r => r.Client)
                .WithMany(r => r.Reviews) // Importante especificar la relación inversa si la hemos definido
                .HasForeignKey(r => r.ClientId);

            });

            modelBuilder.Entity<Request>(request =>
            {
                request.HasKey(r => r.Id);

                request.HasMany(r => r.ImagesProblemRequests)
                    .WithOne(ipr => ipr.Request)
                    .HasForeignKey(r => r.RequestId);
            });
            // Review - Client (Many-to-One)

            // TechnicalProfessionAvailability - Workshop (One-to-Many)
            modelBuilder.Entity<TechnicalProfessionAvailability>(tpa =>
            {
                tpa.HasMany(tpa => tpa.Workshops)
                    .WithOne(w => w.TechnicalProfessionAvailability)
                    .HasForeignKey(w => w.TechnicalProfessionAvailabilityId);

                tpa.HasOne(tpa => tpa.Availability)
                    .WithMany()
                    .HasForeignKey(tpa => tpa.AvailabilityId);
            });
                



            modelBuilder.Entity<Workshop>()
                .HasOne(w => w.TechnicalProfessionAvailability)
                .WithMany(w => w.Workshops) // Importante especificar la relación inversa si la hemos definido
                .HasForeignKey(w => w.TechnicalProfessionAvailabilityId);


            // CategoryService
            modelBuilder.Entity<CategoryService>()
                .HasMany(x => x.ServiceTechnicals)
                .WithOne(x => x.CategoryService)
                .HasForeignKey(x => x.CategoryServiceId);

            // TechnicalResponse
            modelBuilder.Entity<TechnicalResponse>()
                .HasMany(x => x.Materials)
                .WithOne(m => m.TechnicalResponse)
                .HasForeignKey(m => m.TechnicalResponseId);

            //// TechnicalProfessionAvailabilityRequest
            //modelBuilder.Entity<TechnicalProfessionAvailabilityRequest>()
            //    .Has

            modelBuilder.Entity<UserRequest>().HasNoKey();

            // Configuro que entidades quiero que vengan por defecto Habilitadas
            modelBuilder.Entity<User>().HasQueryFilter(x => x.Enabled);
            modelBuilder.Entity<Client>().HasQueryFilter(x => x.Enabled);
            modelBuilder.Entity<Technical>().HasQueryFilter(x => x.Enabled);
            modelBuilder.Entity<UserTechnical>().HasQueryFilter(x => x.Enabled);
            modelBuilder.Entity<WorkshopTechnicalProfession>().HasQueryFilter(x => x.Enabled);


            base.OnModelCreating(modelBuilder);
        }


    }
}
