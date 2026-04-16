using Microsoft.Data.SqlClient;

namespace FileUploadDemo.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
    }

    public class ImageRepository
    {
        private readonly string _connectionString;

        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Image image)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Images (Title, ImagePath) VALUES (" +
                "@title, @path)";
            cmd.Parameters.AddWithValue("@title", image.Title);
            cmd.Parameters.AddWithValue("@path", image.ImagePath);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public List<Image> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images";
            connection.Open();
            var reader = cmd.ExecuteReader();
            List<Image> images = new();
            while (reader.Read())
            {
                images.Add(new Image
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    ImagePath = (string)reader["ImagePath"]
                });
            }

            return images;
        }
    }
}
