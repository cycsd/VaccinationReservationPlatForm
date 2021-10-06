using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VaccinationReservationPlatForm.Models
{
    public partial class VaccinationBookingSystemContext : DbContext
    {
        public VaccinationBookingSystemContext()
        {
        }

        public VaccinationBookingSystemContext(DbContextOptions<VaccinationBookingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<Disease> Diseases { get; set; }
        public virtual DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<HospitalBusinessDay> HospitalBusinessDays { get; set; }
        public virtual DbSet<HospitalBusinessHour> HospitalBusinessHours { get; set; }
        public virtual DbSet<HospitalUser> HospitalUsers { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<UserForHospital> UserForHospitals { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<VaccinationBooking> VaccinationBookings { get; set; }
        public virtual DbSet<VaccinationRecord> VaccinationRecords { get; set; }
        public virtual DbSet<VaccinationTrack> VaccinationTracks { get; set; }
        public virtual DbSet<VaccinationWanted> VaccinationWanteds { get; set; }
        public virtual DbSet<Vaccine> Vaccines { get; set; }
        public virtual DbSet<VaccinePsi> VaccinePsis { get; set; }
        public virtual DbSet<VaccineStock> VaccineStocks { get; set; }
        public virtual DbSet<VaccineToDisease> VaccineToDiseases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VaccinationBookingSystem;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<County>(entity =>
            {
                entity.HasKey(e => e.CountyPostalCode)
                    .HasName("PK__County__D2A1E21283E13CC8");

                entity.ToTable("County");

                entity.HasIndex(e => e.CountyName, "INDEX_CountyName");

                entity.HasIndex(e => e.CountyTownName, "INDEX_CountyTownName");

                entity.Property(e => e.CountyPostalCode).ValueGeneratedNever();

                entity.Property(e => e.CountyName)
                    .HasMaxLength(3)
                    .IsFixedLength(true);

                entity.Property(e => e.CountyTownName)
                    .HasMaxLength(3)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Disease>(entity =>
            {
                entity.ToTable("Disease");

                entity.HasIndex(e => e.DiseaseName, "DiseaseName")
                    .IsUnique();

                entity.HasIndex(e => e.DiseaseName, "UQ__Disease__5112584DD48ACF62")
                    .IsUnique();

                entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");

                entity.Property(e => e.DiseaseCategoryId).HasColumnName("DiseaseCategoryID");

                entity.Property(e => e.DiseaseName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DiseaseCategory)
                    .WithMany(p => p.Diseases)
                    .HasForeignKey(d => d.DiseaseCategoryId)
                    .HasConstraintName("FK__Disease__Disease__300424B4");
            });

            modelBuilder.Entity<DiseaseCategory>(entity =>
            {
                entity.ToTable("DiseaseCategory");

                entity.HasIndex(e => e.DiseaseCategoryName, "DiseaseCategoryName")
                    .IsUnique();

                entity.HasIndex(e => e.DiseaseCategoryName, "UQ__DiseaseC__3D6E551C69A7B00C")
                    .IsUnique();

                entity.Property(e => e.DiseaseCategoryId).HasColumnName("DiseaseCategoryID");

                entity.Property(e => e.DiseaseCategoryName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.ToTable("Hospital");

                entity.HasIndex(e => e.HospitalName, "INDEX_HospitalName")
                    .IsUnique();

                entity.HasIndex(e => e.HospitalName, "UQ__Hospital__08A92CEFDED3F899")
                    .IsUnique();

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.HospitalAdress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HospitalMail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HospitalName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.HospitalPhone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.HospitalPhoto).HasColumnType("image");

                entity.HasOne(d => d.CountyPostalCodeNavigation)
                    .WithMany(p => p.Hospitals)
                    .HasForeignKey(d => d.CountyPostalCode)
                    .HasConstraintName("FK__Hospital__County__3B75D760");
            });

            modelBuilder.Entity<HospitalBusinessDay>(entity =>
            {
                entity.ToTable("HospitalBusinessDay");

                entity.HasIndex(e => e.HospitalId, "INDEX_HospitalIDinHospitalBusinessDay");

                entity.HasIndex(e => new { e.HospitalId, e.Hbdweekday }, "UQ_HospitalID_HBDweekday")
                    .IsUnique();

                entity.Property(e => e.HospitalBusinessDayId).HasColumnName("HospitalBusinessDayID");

                entity.Property(e => e.Hbdmark).HasColumnName("HBDmark");

                entity.Property(e => e.Hbdweekday).HasColumnName("HBDweekday");

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.HospitalBusinessDays)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__HospitalB__Hospi__534D60F1");
            });

            modelBuilder.Entity<HospitalBusinessHour>(entity =>
            {
                entity.ToTable("HospitalBusinessHour");

                entity.HasIndex(e => e.HospitalBusinessDayId, "INDEX_HospitalBusinessDayIDinHospitalBusinessHour");

                entity.HasIndex(e => new { e.HospitalBusinessDayId, e.HbhstartTime }, "UQ_HospitalBusinessDayID_HBHstartTime")
                    .IsUnique();

                entity.Property(e => e.HospitalBusinessHourId).HasColumnName("HospitalBusinessHourID");

                entity.Property(e => e.HbhendTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("HBHendTime");

                entity.Property(e => e.HbhstartTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("HBHstartTime");

                entity.Property(e => e.HospitalBusinessDayId).HasColumnName("HospitalBusinessDayID");

                entity.HasOne(d => d.HospitalBusinessDay)
                    .WithMany(p => p.HospitalBusinessHours)
                    .HasForeignKey(d => d.HospitalBusinessDayId)
                    .HasConstraintName("FK__HospitalB__Hospi__571DF1D5");
            });

            modelBuilder.Entity<HospitalUser>(entity =>
            {
                entity.ToTable("HospitalUser");

                entity.HasIndex(e => e.HospitalUserName, "INDEX_HospitalUserName")
                    .IsUnique();

                entity.HasIndex(e => e.HospitalUserName, "UQ__Hospital__4D9099F1E7EB59DF")
                    .IsUnique();

                entity.Property(e => e.HospitalUserId).HasColumnName("HospitalUserID");

                entity.Property(e => e.HospitalUserName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.HospitalUserPassword).HasMaxLength(128);

                entity.Property(e => e.HospitalUserPasswordSalt)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.HasIndex(e => e.PersonIdentityId, "PersonCheck")
                    .IsUnique();

                entity.HasIndex(e => e.PersonHealthId, "UQ__Person__0C498A2814CD9A7A")
                    .IsUnique();

                entity.HasIndex(e => e.PersonIdentityId, "UQ__Person__9378170600E05912")
                    .IsUnique();

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.PersonAdress).HasMaxLength(40);

                entity.Property(e => e.PersonBirthday).HasColumnType("smalldatetime");

                entity.Property(e => e.PersonCellphoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PersonHealthId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("PersonHealthID")
                    .IsFixedLength(true);

                entity.Property(e => e.PersonIdentityId)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PersonIdentityID")
                    .IsFixedLength(true);

                entity.Property(e => e.PersonIdphoto)
                    .HasColumnType("image")
                    .HasColumnName("PersonIDphoto");

                entity.Property(e => e.PersonJob)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonMail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonName).HasMaxLength(20);

                entity.Property(e => e.PersonPassword).HasMaxLength(128);

                entity.Property(e => e.PersonPasswordSalt)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PersonSex)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.HasOne(d => d.CountyPostalCodeNavigation)
                    .WithMany(p => p.People)
                    .HasForeignKey(d => d.CountyPostalCode)
                    .HasConstraintName("FK__Person__CountyPo__29572725");
            });

            modelBuilder.Entity<UserForHospital>(entity =>
            {
                entity.ToTable("UserForHospital");

                entity.HasIndex(e => e.HospitalUserId, "INDEX_HospitalUserIDinUserForHosspital");

                entity.HasIndex(e => new { e.HospitalUserId, e.HospitalId }, "UQ_UserConnectHospital")
                    .IsUnique();

                entity.Property(e => e.UserForHospitalId).HasColumnName("UserForHospitalID");

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.HospitalUserId).HasColumnName("HospitalUserID");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.UserForHospitals)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__UserForHo__Hospi__403A8C7D");

                entity.HasOne(d => d.HospitalUser)
                    .WithMany(p => p.UserForHospitals)
                    .HasForeignKey(d => d.HospitalUserId)
                    .HasConstraintName("FK__UserForHo__Hospi__3F466844");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("UserInfo");

                entity.HasIndex(e => e.PersonId, "INDEX_PersonIDinUserInfo");

                entity.HasIndex(e => new { e.PersonId, e.DiseaseId }, "UQ_PersonDisease")
                    .IsUnique();

                entity.Property(e => e.UserInfoId).HasColumnName("UserInfoID");

                entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.UserInfoDiseaseDetail).HasMaxLength(50);

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.UserInfos)
                    .HasForeignKey(d => d.DiseaseId)
                    .HasConstraintName("FK__UserInfo__Diseas__34C8D9D1");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.UserInfos)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK__UserInfo__Person__33D4B598");
            });

            modelBuilder.Entity<VaccinationBooking>(entity =>
            {
                entity.ToTable("VaccinationBooking");

                entity.HasIndex(e => new { e.HospitalId, e.VbbookingDate }, "INDEX_HospitalID_VBbookingDate");

                entity.HasIndex(e => e.PersonId, "INDEX_PersonIDinVaccinationBooking");

                entity.HasIndex(e => new { e.PersonId, e.VaccineId }, "UQ_PersonID_VaccineID")
                    .IsUnique();

                entity.Property(e => e.VaccinationBookingId).HasColumnName("VaccinationBookingID");

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.VbappointmentDate)
                    .HasColumnType("date")
                    .HasColumnName("VBappointmentDate");

                entity.Property(e => e.VbbookingDate)
                    .HasColumnType("date")
                    .HasColumnName("VBbookingDate");

                entity.Property(e => e.VbbookingTime)
                    .HasColumnType("time(0)")
                    .HasColumnName("VBbookingTime");

                entity.Property(e => e.VbchargeRemark)
                    .HasMaxLength(2)
                    .HasColumnName("VBchargeRemark")
                    .IsFixedLength(true);

                entity.Property(e => e.VbcheckRemark)
                    .HasMaxLength(3)
                    .HasColumnName("VBcheckRemark")
                    .IsFixedLength(true);

                entity.Property(e => e.VbclickMoment)
                    .HasColumnType("datetime")
                    .HasColumnName("VBclickMoment");

                entity.Property(e => e.Vbnumber).HasColumnName("VBnumber");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.VaccinationBookings)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__Vaccinati__Hospi__5BE2A6F2");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.VaccinationBookings)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK__Vaccinati__Perso__5AEE82B9");

                entity.HasOne(d => d.Vaccine)
                    .WithMany(p => p.VaccinationBookings)
                    .HasForeignKey(d => d.VaccineId)
                    .HasConstraintName("FK__Vaccinati__Vacci__5CD6CB2B");
            });

            modelBuilder.Entity<VaccinationRecord>(entity =>
            {
                entity.ToTable("VaccinationRecord");

                entity.HasIndex(e => e.PersonId, "INDEX_PersonIDinVaccinationRecord");

                entity.Property(e => e.VaccinationRecordId).HasColumnName("VaccinationRecordID");

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.VrgivenDate)
                    .HasColumnType("date")
                    .HasColumnName("VRgivenDate");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.VaccinationRecords)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__Vaccinati__Hospi__60A75C0F");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.VaccinationRecords)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK__Vaccinati__Perso__5FB337D6");

                entity.HasOne(d => d.Vaccine)
                    .WithMany(p => p.VaccinationRecords)
                    .HasForeignKey(d => d.VaccineId)
                    .HasConstraintName("FK__Vaccinati__Vacci__619B8048");
            });

            modelBuilder.Entity<VaccinationTrack>(entity =>
            {
                entity.ToTable("VaccinationTrack");

                entity.HasIndex(e => e.PersonId, "INDEX_PersonIDinVaccinationTrack");

                entity.HasIndex(e => new { e.PersonId, e.VaccineId }, "UQ_PersonID_VaccineIDinVaccinationTrack")
                    .IsUnique();

                entity.Property(e => e.VaccinationTrackId).HasColumnName("VaccinationTrackID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.VtappointmentDate)
                    .HasColumnType("date")
                    .HasColumnName("VTappointmentDate");

                entity.Property(e => e.Vttimes).HasColumnName("VTtimes");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.VaccinationTracks)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK__Vaccinati__Perso__656C112C");

                entity.HasOne(d => d.Vaccine)
                    .WithMany(p => p.VaccinationTracks)
                    .HasForeignKey(d => d.VaccineId)
                    .HasConstraintName("FK__Vaccinati__Vacci__66603565");
            });

            modelBuilder.Entity<VaccinationWanted>(entity =>
            {
                entity.ToTable("VaccinationWanted");

                entity.HasIndex(e => new { e.PersonId, e.VaccineId }, "INDEX_PersonID_VaccineIDinVaccinationWanted")
                    .IsUnique();

                entity.HasIndex(e => new { e.PersonId, e.VaccineId }, "UQ_PersonID_VaccineIDinVaccinationWanted")
                    .IsUnique();

                entity.Property(e => e.VaccinationWantedId).HasColumnName("VaccinationWantedID");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.VwassignMark)
                    .HasMaxLength(3)
                    .HasColumnName("VWassignMark")
                    .IsFixedLength(true);

                entity.Property(e => e.VwleftoverWillingness)
                    .HasMaxLength(1)
                    .HasColumnName("VWleftoverWillingness")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.VaccinationWanteds)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK__Vaccinati__Perso__6A30C649");

                entity.HasOne(d => d.Vaccine)
                    .WithMany(p => p.VaccinationWanteds)
                    .HasForeignKey(d => d.VaccineId)
                    .HasConstraintName("FK__Vaccinati__Vacci__6B24EA82");
            });

            modelBuilder.Entity<Vaccine>(entity =>
            {
                entity.ToTable("Vaccine");

                entity.HasIndex(e => e.VaccineName, "INDEX_VaccineName")
                    .IsUnique();

                entity.HasIndex(e => e.VaccineName, "UQ__Vaccine__CAD609D5060CADD5")
                    .IsUnique();

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.VaccineExp).HasColumnName("VaccineEXP");

                entity.Property(e => e.VaccineName)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.VaccineOfficialName).HasMaxLength(30);

                entity.Property(e => e.VaccineStoredInfo).HasMaxLength(150);
            });

            modelBuilder.Entity<VaccinePsi>(entity =>
            {
                entity.ToTable("VaccinePSI");

                entity.HasIndex(e => new { e.HospitalId, e.VaccineId }, "INDEX_HospitalID_VaccineID");

                entity.Property(e => e.VaccinePsiid).HasColumnName("VaccinePSIID");

                entity.Property(e => e.BatchNumber)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.VaccinePsiexpDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("VaccinePSIexpDate");

                entity.Property(e => e.VaccinePsimfDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("VaccinePSImfDate");

                entity.Property(e => e.VaccinePsiquantity).HasColumnName("VaccinePSIquantity");

                entity.Property(e => e.VaccinePsirecordTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("VaccinePSIrecordTime");

                entity.Property(e => e.VaccinePsistateRemark)
                    .HasMaxLength(5)
                    .HasColumnName("VaccinePSIstateRemark")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.VaccinePsis)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__VaccinePS__Hospi__4E88ABD4");

                entity.HasOne(d => d.Vaccine)
                    .WithMany(p => p.VaccinePsis)
                    .HasForeignKey(d => d.VaccineId)
                    .HasConstraintName("FK__VaccinePS__Vacci__4F7CD00D");
            });

            modelBuilder.Entity<VaccineStock>(entity =>
            {
                entity.ToTable("VaccineStock");

                entity.HasIndex(e => e.HospitalId, "INDEX_HospitalIDinVaccineStock");

                entity.HasIndex(e => new { e.HospitalId, e.VaccineId }, "UQ_HospitalVaccine")
                    .IsUnique();

                entity.Property(e => e.VaccineStockId).HasColumnName("VaccineStockID");

                entity.Property(e => e.HospitalId).HasColumnName("HospitalID");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.VaccineStocks)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__VaccineSt__Hospi__4AB81AF0");

                entity.HasOne(d => d.Vaccine)
                    .WithMany(p => p.VaccineStocks)
                    .HasForeignKey(d => d.VaccineId)
                    .HasConstraintName("FK__VaccineSt__Vacci__4BAC3F29");
            });

            modelBuilder.Entity<VaccineToDisease>(entity =>
            {
                entity.ToTable("VaccineToDisease");

                entity.HasIndex(e => e.DiseaseId, "INDEX_DiseaseIDinVaccineToDisease");

                entity.HasIndex(e => e.VaccineId, "INDEX_VaccineIDinVaccineToDisease");

                entity.Property(e => e.VaccineToDiseaseId).HasColumnName("VaccineToDiseaseID");

                entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");

                entity.Property(e => e.VaccineId).HasColumnName("VaccineID");

                entity.Property(e => e.VtdrequiredNumber).HasColumnName("VTDrequiredNumber");

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.VaccineToDiseases)
                    .HasForeignKey(d => d.DiseaseId)
                    .HasConstraintName("FK__VaccineTo__Disea__46E78A0C");

                entity.HasOne(d => d.Vaccine)
                    .WithMany(p => p.VaccineToDiseases)
                    .HasForeignKey(d => d.VaccineId)
                    .HasConstraintName("FK__VaccineTo__Vacci__45F365D3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
