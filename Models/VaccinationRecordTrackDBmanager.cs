using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccinationReservationPlatForm.ViewModel;

namespace VaccinationReservationPlatForm.Models
{
    public class VaccinationRecordTrackDBmanager
    {
        private readonly string ConnStr = "Data Source=.;Initial Catalog=VaccinationBookingSystem;Integrated Security=True";
        public List<VaccinationRecordTrackViewModel> GetRecord(int personID)
        {
            List<VaccinationRecordTrackViewModel> records = new List<VaccinationRecordTrackViewModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(
                "SELECT vr.VRgivenDate, v.VaccineName, d.DiseaseName, h.HospitalName " +
                "FROM VaccinationRecord as vr " +
                "JOIN VaccineToDisease as vtd ON(vr.VaccineID = vtd.VaccineID) " +
                "JOIN Vaccine as v ON(vr.VaccineID = v.VaccineID) " +
                "JOIN Disease as d ON(vtd.DiseaseID = d.DiseaseID) " +
                "JOIN Hospital as h ON(vr.HospitalID = h.HospitalID) " +
                $"where vr.PersonID = {personID}"
                );
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    VaccinationRecordTrackViewModel record = new VaccinationRecordTrackViewModel
                    {
                        VRgivenDate = (reader.GetDateTime(reader.GetOrdinal("VRgivenDate"))).GetDateTimeFormats('D')[0].ToString(), //接踵日期
                        DiseaseName = reader.GetString(reader.GetOrdinal("DiseaseName")), //疫苗種類
                        VaccineName = reader.GetString(reader.GetOrdinal("VaccineName")), //疫苗品名
                        HospitalName = reader.GetString(reader.GetOrdinal("HospitalName")), //施打單位
                    };
                    records.Add(record);
                }
            }
            else
            {

            }
            sqlConnection.Close();
            return records;

        }
    }
}