using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace KTN5.Models
{
    public class RestaurantFactory
    {
        public Restaurant queryByFid(int rId)
        {
            List<Restaurant> list = queryBySql("SELECT * FROM Restaurant WHERE rId=" + rId.ToString());
            if (list.Count == 0)
                return null;
            return list[0];
        }


        public List<Restaurant> queryAll()
        {
            return queryBySql("SELECT * FROM Restaurant");
        }

        internal List<Restaurant> queryByKeyword(string keyword, string area)
        {
            string sql = "SELECT * FROM Restaurant WHERE ((rName LIKE '%" + keyword + "%')";
            sql += " OR (rPhone LIKE '%" + keyword + "%')";
            sql += " OR (rAddress LIKE '%" + keyword + "%')";
            sql += " OR (startTime LIKE '%" + keyword + "%')";
            sql += " OR (endTime LIKE '%" + keyword + "%'))";
            sql += " AND (rAddress LIKE '%" + area + "%')";
            return queryBySql(sql);
        }

        //下拉式搜尋地區
        public List<Restaurant> queryByArea(string area)
        {
            string sql = "SELECT * FROM Restaurant WHERE rAddress LIKE '%" + area + "%'";
            return queryBySql(sql);
        }

        internal Restaurant queryByPhone(string rPhone)
        {
            List<Restaurant> list = queryBySql("SELECT * FROM Restaurant WHERE rPhone='" + rPhone + "'");
            if (list.Count == 0)
                return null;
            return list[0];
        }

        internal Restaurant queryByAddress(string rAddress)
        {
            List<Restaurant> list = queryBySql("SELECT * FROM Restaurant WHERE rAddress='" + rAddress + "'");
            if (list.Count == 0)
                return null;
            return list[0];
        }

        internal Restaurant queryBystartTime(TimeSpan startTime)
        {
            List<Restaurant> list = queryBySql("SELECT * FROM Restaurant WHERE startTime='" + startTime + "'");
            if (list.Count == 0)
                return null;
            return list[0];
        }

        internal Restaurant queryByendTime(TimeSpan endTime)
        {
            List<Restaurant> list = queryBySql("SELECT * FROM Restaurant WHERE endTime='" + endTime + "'");
            if (list.Count == 0)
                return null;
            return list[0];
        }

        public List<Restaurant> queryBySql(string sql)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbktn;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Restaurant> list = new List<Restaurant>();
            while (reader.Read())
            {
                Restaurant x = new Restaurant()
                {
                    rId = (int)reader["rId"],
                    rName = reader["rName"].ToString(),
                    rPhone = reader["rPhone"].ToString(),
                    rAddress = reader["rAddress"].ToString(),
                    startTime = (TimeSpan)reader["startTime"],
                    endTime = (TimeSpan)reader["endTime"]
                };
                list.Add(x);
            }
            con.Close();
            return list;
        }
        public void delete(int rId)
        {
            string sql = "DELETE FROM Restaurant WHERE rId=" + rId.ToString();
            executeSql(sql);
        }
        public void update(Restaurant p)
        {
            string sql = "UPDATE Restaurant SET ";
            sql += "rName='" + p.rName + "',";
            sql += "rPhone='" + p.rPhone + "',";
            sql += "rAddress='" + p.rAddress + "',";
            sql += "startTime='" + p.startTime + "',";
            sql += "endTime='" + p.endTime + "'";
            sql += "WHERE rId=" + p.rId.ToString();

            executeSql(sql);
        }
        public void create(Restaurant p)
        {
            string sql = "INSERT INTO Restaurant (";
            sql += "rName,";
            sql += "rPhone,";
            sql += "rAddress,";
            sql += "startTime,";
            sql += "endTime";
            sql += ")VALUES(";
            sql += "'" + p.rName + "',";
            sql += "'" + p.rPhone + "',";
            sql += "'" + p.rAddress + "',";
            sql += "'" + p.startTime + "',";
            sql += "'" + p.endTime + "')";

            executeSql(sql);
        }

        private void executeSql(string sql)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbktn;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}