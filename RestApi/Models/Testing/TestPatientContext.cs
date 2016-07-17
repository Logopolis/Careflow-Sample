using System;
using System.Data.Entity;

namespace RestApi.Models.Testing
{
    public class TestPatientContext : IPatientContext
    {
        private readonly TestDbSet<Episode> _episodes;
        private readonly TestDbSet<Patient> _patients;

        public TestPatientContext()
        {
            _episodes = new TestDbSet<Episode>();
            _patients = new TestDbSet<Patient>();
        }

        public DbSet<Episode> Episodes
        {
            get
            {
                return _episodes;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public DbSet<Patient> Patients
        {
            get
            {
                return _patients;
            }

            set
            {
                throw new NotSupportedException();
            }
        }
    }
}