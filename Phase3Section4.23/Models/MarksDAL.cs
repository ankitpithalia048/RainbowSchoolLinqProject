using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Phase3Section4._23.Models
{
    public class MarksDAL
    {
        string connectionString = "Server=BSC-F8JTXC2\\SQLEXPRESS; Database=School1; Trusted_Connection=True;";

        public IEnumerable<Marks> GetAllMarks()
        {
            List<Marks> lstMarks = new List<Marks>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from Marks", con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Marks marks = new Marks();

                    marks.Student_ID = Convert.ToInt32(rdr["Student_ID"]);
                    marks.Subject_ID = Convert.ToInt32(rdr["Subject_ID"]);
                    marks.Value = Convert.ToInt32(rdr["Value"]);



                    lstMarks.Add(marks);
                }
                con.Close();
            }
            return lstMarks;
        }
    }
}
