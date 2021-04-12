using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phase3Section4._23.Models
{
    public class SubjectDAL
    {
        string connectionString = "Server=BSC-F8JTXC2\\SQLEXPRESS; Database=School1; Trusted_Connection=True;";

        public IEnumerable<Subject> GetAllSubjects()
        {
            List<Subject> lstSubjects = new List<Subject>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from Subject", con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Subject subject = new Subject();

                    subject.ID = Convert.ToInt32(rdr["ID"]);
                    subject.Name = rdr["Name"].ToString();
                    subject.StudentId = Convert.ToInt32(rdr["StudentID"]);

                    lstSubjects.Add(subject);
                }
                con.Close();
            }
            return lstSubjects;
        }

    }
}
