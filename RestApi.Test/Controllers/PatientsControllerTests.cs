using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Models;
using RestApi.Models.Testing;
using System;
using System.Net;
using System.Web.Http;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class PatientsControllerTests
    {
        private PatientsController _controller;
        private IPatientContext _patientContext;

        [SetUp]
        public void SetUp()
        {
            _patientContext = new TestPatientContext();
            
            _controller = _GetController(_patientContext);
        }

        [Test]
        public void ShouldReturnPatientWithEpisode()
        {
            var patient = new Patient
            {
                DateOfBirth = new DateTime(2000, 12, 10),
                FirstName = "Mickey",
                LastName = "Mouse",
                NhsNumber = "MM1234",
                PatientId = 1
            };
            _patientContext.Patients.Add(patient);

            var episode = new Episode
            {
                AdmissionDate = new DateTime(2016, 1, 2),
                Diagnosis = "Fractured jaw",
                DischargeDate = new DateTime(2016, 1, 3),
                PatientId = 1,
                EpisodeId = 100
            };
            _patientContext.Episodes.Add(episode);

            var outcome = _controller.Get(1);

            var expected = new Patient
            {
                DateOfBirth = patient.DateOfBirth,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                NhsNumber = patient.NhsNumber,
                PatientId = patient.PatientId,
                Episodes = new []
                {
                    new Episode
                    {
                        AdmissionDate = episode.AdmissionDate,
                        Diagnosis = episode.Diagnosis,
                        DischargeDate = episode.DischargeDate,
                        PatientId = episode.PatientId,
                        EpisodeId = episode.EpisodeId
                    }
                }
            };

            // TODO: Refactor this equality comparing logic into something re-usable.
            var expectedJson = Newtonsoft.Json.JsonConvert.SerializeObject(expected);
            var outcomeJson = Newtonsoft.Json.JsonConvert.SerializeObject(outcome);
            Assert.That(expectedJson, Is.EqualTo(outcomeJson));
        }

        [Test]
        public void ShouldThrow404WhenNoMatchingPatient()
        {
            var patient = new Patient
            {
                DateOfBirth = new DateTime(2000, 12, 10),
                FirstName = "Minnie",
                LastName = "Mouse",
                NhsNumber = "MM5678",
                PatientId = 2
            };
            _patientContext.Patients.Add(patient);

            var episode = new Episode
            {
                AdmissionDate = new DateTime(2016, 1, 2),
                Diagnosis = "Fractured jaw",
                DischargeDate = new DateTime(2016, 1, 3),
                PatientId = 2,
                EpisodeId = 101
            };
            _patientContext.Episodes.Add(episode);

            var httpResponseException = Assert.Throws<HttpResponseException>(() =>
                _controller.Get(63)); // Shouldn't match PatientId = 2

            Assert.That(httpResponseException.Response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        
        // TODO: Turn this into a re-usable CastleWindsorComponentResolver which retrieves any registered component
        private PatientsController _GetController(IPatientContext patientContextToInject)
        {
            var container = WebApiApplication.ConfigureWindsor(GlobalConfiguration.Configuration);
            return container.Resolve<PatientsController>(new { patientContext = patientContextToInject });
        }
    }
}
