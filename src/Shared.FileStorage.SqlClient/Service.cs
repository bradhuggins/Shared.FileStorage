#region Using Statements
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
#endregion

namespace Shared.FileStorage.SqlClient
{
    public class Service : IService
    {
        public string BasePath { get; set; }

        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new System.Exception("Error: ConnectionString not set!");
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        public async Task DeleteFile(string shareName, string folder, string fileName)
        {
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM _ApplicationFiles WHERE ShareName=@ShareName AND FolderName=@FolderName AND FileName=@FileName";
                cmd.Parameters.Add(new SqlParameter("@ShareName", shareName));
                cmd.Parameters.Add(new SqlParameter("@FolderName", folder));
                cmd.Parameters.Add(new SqlParameter("@FileName", fileName));

                conn.Open();
                int result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<byte[]> DownloadFile(string shareName, string folder, string fileName)
        {
            byte[] toReturn = null;
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT ByteData FROM _ApplicationFiles (NOLOCK) WHERE ShareName=@ShareName AND FolderName=@FolderName AND FileName=@FileName";
                cmd.Parameters.Add(new SqlParameter("@ShareName", shareName));
                cmd.Parameters.Add(new SqlParameter("@FolderName", folder));
                cmd.Parameters.Add(new SqlParameter("@FileName", fileName));

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                    if (reader != null)
                    {
                        reader.Read();
                        toReturn = new Byte[(reader.GetBytes(0, 0, null, 0, int.MaxValue))];
                        reader.GetBytes(0, 0, toReturn, 0, toReturn.Length);
                    }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            finally
            {
                conn.Close();
            }
            return toReturn;
        }

        public async Task<List<string>> ListFiles(string shareName, string folder)
        {
            List<string> toReturn = new List<string>();
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT FileName FROM _ApplicationFiles (NOLOCK) WHERE ShareName=@ShareName AND FolderName=@FolderName ORDER BY FileName DESC";
                cmd.Parameters.Add(new SqlParameter("@ShareName", shareName));
                cmd.Parameters.Add(new SqlParameter("@FolderName", folder));

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toReturn.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            finally
            {
                conn.Close();
            }
            return toReturn;
        }

        public async Task<List<string>> ListFolders(string shareName)
        {
            List<string> toReturn = new List<string>();
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                if (string.IsNullOrEmpty(shareName))
                {
                    cmd.CommandText = "SELECT ShareName FROM _ApplicationFiles (NOLOCK) ORDER BY ShareName DESC";
                }
                else
                {
                    cmd.CommandText = "SELECT FolderName FROM _ApplicationFiles (NOLOCK) WHERE ShareName=@ShareName ORDER BY FolderName DESC";
                    cmd.Parameters.Add(new SqlParameter("@ShareName", shareName));
                }

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toReturn.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            finally
            {
                conn.Close();
            }
            return toReturn;
        }

        public async Task UploadFile(string shareName, string folder, string fileName, byte[] fileData)
        {
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            try
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO _ApplicationFiles (ShareName,FolderName,FileName,ByteData) Values(@ShareName,@FolderName,@FileName,@ByteData)", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@ShareName", shareName));
                    cmd.Parameters.Add(new SqlParameter("@FolderName", folder));
                    cmd.Parameters.Add(new SqlParameter("@FileName", fileName));
                    cmd.Parameters.Add(new SqlParameter("@ByteData", fileData));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
