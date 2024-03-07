using EpiLogistic.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EpiLogistic.Controllers
{
    public class SpedizioneController : Controller
    {
        private readonly string connectionString = "Data Source=GABRIELE-PORTAT\\SQLEXPRESS;Initial Catalog=EpiLogistic;Integrated Security=True;";

        // GET: Spedizione
        public ActionResult Index()
        {
            List<SpedizioneModel> spedizioni = GetSpedizioniFromDatabase();
            return View(spedizioni);
        }

        // GET: Spedizione/Details/5
        public ActionResult Details(int id)
        {
            SpedizioneModel spedizione = GetSpedizioneById(id);

            if (spedizione == null)
            {
                return HttpNotFound();
            }

            return View(spedizione);
        }

        // GET: Spedizione/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Spedizione/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SpedizioneModel spedizione)
        {
            if (ModelState.IsValid)
            {
                InsertSpedizioneIntoDatabase(spedizione);
                return RedirectToAction("Index");
            }

            return View(spedizione);
        }

        // GET: Spedizione/Edit/5
        public ActionResult Edit(int id)
        {
            SpedizioneModel spedizione = GetSpedizioneById(id);

            if (spedizione == null)
            {
                return HttpNotFound();
            }

            return View(spedizione);
        }

        // POST: Spedizione/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SpedizioneModel spedizione)
        {
            if (ModelState.IsValid)
            {
                UpdateSpedizioneInDatabase(spedizione);
                return RedirectToAction("Index");
            }

            return View(spedizione);
        }

        // GET: Spedizione/Delete/5
        public ActionResult Delete(int id)
        {
            SpedizioneModel spedizione = GetSpedizioneById(id);

            if (spedizione == null)
            {
                return HttpNotFound();
            }

            return View(spedizione);
        }

        // POST: Spedizione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeleteSpedizioneFromDatabase(id);
            return RedirectToAction("Index");
        }

        private List<SpedizioneModel> GetSpedizioniFromDatabase()
        {
            List<SpedizioneModel> spedizioni = new List<SpedizioneModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Spedizioni", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SpedizioneModel spedizione = new SpedizioneModel
                            {
                                Id = (int)reader["Id"],
                                ClienteId = (int)reader["ClienteId"],
                                NumeroIdentificativo = (int)reader["NumeroIdentificativo"],
                                DataSpedizione = (DateTime)reader["DataSpedizione"],
                                Peso = (decimal)reader["Peso"],
                                CittaDestinataria = reader["CittaDestinataria"].ToString(),
                                IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                                NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                                Costo = (decimal)reader["Costo"],
                                DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"]
                            };

                            spedizioni.Add(spedizione);
                        }
                    }
                }
            }

            return spedizioni;
        }

        private SpedizioneModel GetSpedizioneById(int id)
        {
            SpedizioneModel spedizione = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Spedizioni WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            spedizione = new SpedizioneModel
                            {
                                Id = (int)reader["Id"],
                                ClienteId = (int)reader["ClienteId"],
                                NumeroIdentificativo = (int)reader["NumeroIdentificativo"],
                                DataSpedizione = (DateTime)reader["DataSpedizione"],
                                Peso = (decimal)reader["Peso"],
                                CittaDestinataria = reader["CittaDestinataria"].ToString(),
                                IndirizzoDestinatario = reader["IndirizzoDestinatario"].ToString(),
                                NominativoDestinatario = reader["NominativoDestinatario"].ToString(),
                                Costo = (decimal)reader["Costo"],
                                DataConsegnaPrevista = (DateTime)reader["DataConsegnaPrevista"]
                            };
                        }
                    }
                }
            }

            return spedizione;
        }

        private void InsertSpedizioneIntoDatabase(SpedizioneModel spedizione)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO Spedizioni (ClienteId, NumeroIdentificativo, DataSpedizione, Peso, CittaDestinataria, " +
                    "IndirizzoDestinatario, NominativoDestinatario, Costo, DataConsegnaPrevista) " +
                    "VALUES (@ClienteId, @NumeroIdentificativo, @DataSpedizione, @Peso, @CittaDestinataria, " +
                    "@IndirizzoDestinatario, @NominativoDestinatario, @Costo, @DataConsegnaPrevista)", connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", spedizione.ClienteId);
                    command.Parameters.AddWithValue("@NumeroIdentificativo", spedizione.NumeroIdentificativo);
                    command.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                    command.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    command.Parameters.AddWithValue("@CittaDestinataria", spedizione.CittaDestinataria);
                    command.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                    command.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                    command.Parameters.AddWithValue("@Costo", spedizione.Costo);
                    command.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateSpedizioneInDatabase(SpedizioneModel spedizione)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "UPDATE Spedizioni SET ClienteId = @ClienteId, NumeroIdentificativo = @NumeroIdentificativo, " +
                    "DataSpedizione = @DataSpedizione, Peso = @Peso, CittaDestinataria = @CittaDestinataria, " +
                    "IndirizzoDestinatario = @IndirizzoDestinatario, NominativoDestinatario = @NominativoDestinatario, " +
                    "Costo = @Costo, DataConsegnaPrevista = @DataConsegnaPrevista WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", spedizione.ClienteId);
                    command.Parameters.AddWithValue("@NumeroIdentificativo", spedizione.NumeroIdentificativo);
                    command.Parameters.AddWithValue("@DataSpedizione", spedizione.DataSpedizione);
                    command.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    command.Parameters.AddWithValue("@CittaDestinataria", spedizione.CittaDestinataria);
                    command.Parameters.AddWithValue("@IndirizzoDestinatario", spedizione.IndirizzoDestinatario);
                    command.Parameters.AddWithValue("@NominativoDestinatario", spedizione.NominativoDestinatario);
                    command.Parameters.AddWithValue("@Costo", spedizione.Costo);
                    command.Parameters.AddWithValue("@DataConsegnaPrevista", spedizione.DataConsegnaPrevista);
                    command.Parameters.AddWithValue("@Id", spedizione.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteSpedizioneFromDatabase(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM Spedizioni WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

}