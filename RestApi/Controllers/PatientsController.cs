using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using RestApi.Models;

namespace RestApi.Controllers
{
    public class PatientsController : ApiController
    {
        [HttpGet]
        public Patient Get(int patientId)
        {
            var patientContext = new PatientContext();

            var patientsAndEpisodes =
                from p in patientContext.Patients
                join e in patientContext.Episodes on p.PatientId equals e.PatientId
                where p.PatientId == patientId
                select new {p, e};

            if (patientsAndEpisodes.Any())
            {
                var first = patientsAndEpisodes.First().p;
                first.Episodes = patientsAndEpisodes.Select(x => x.e).ToArray();
                return first;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}