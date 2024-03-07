using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using EpiLogistic.Models; // Assicurati di aggiungere il namespace corretto

namespace EpiLogistic.Controllers
{
    public class AggiornamentoSpedizioneController : Controller
    {
        private readonly string connectionString = "Data Source=GABRIELE-PORTAT\\SQLEXPRESS;Initial Catalog=EpiLogistic;Integrated Security=True;";

        // GET: AggiornamentoSpedizione
        public ActionResult Index()
        {
            return View();
        }

        // GET: AggiornamentoSpedizione/Update
        public ActionResult Update()
        {
            return View();
        }

        // GET: AggiornamentoSpedizione/IndexWithId
        public ActionResult IndexWithId(int spedizioneId)
        {
            List<AggiornamentoSpedizioneModel> aggiornamenti = GetAggiornamentiFromDatabase(spedizioneId);
            return View(aggiornamenti);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AggiungiAggiornamento(int spedizioneId, string stato, string luogo, string descrizione)
        {
            try
            {
                AggiornamentoSpedizioneModel nuovoAggiornamento = new AggiornamentoSpedizioneModel
                {
                    SpedizioneId = spedizioneId,
                    Stato = stato,
                    Luogo = luogo,
                    Descrizione = descrizione,
                    DataOraAggiornamento = DateTime.Now 
                };

                InserisciAggiornamentoNelDatabase(nuovoAggiornamento);

                return RedirectToAction("Index", new { spedizioneId = spedizioneId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Si è verificato un errore durante l'aggiunta dell'aggiornamento.";
                return View("Error");
            }
        }

        private void InserisciAggiornamentoNelDatabase(AggiornamentoSpedizioneModel aggiornamento)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO AggiornamentiSpedizioni (SpedizioneId, Stato, Luogo, Descrizione, DataOraAggiornamento) " +
                               "VALUES (@SpedizioneId, @Stato, @Luogo, @Descrizione, @DataOraAggiornamento)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SpedizioneId", aggiornamento.SpedizioneId);
                    command.Parameters.AddWithValue("@Stato", aggiornamento.Stato);
                    command.Parameters.AddWithValue("@Luogo", aggiornamento.Luogo);
                    command.Parameters.AddWithValue("@Descrizione", aggiornamento.Descrizione);
                    command.Parameters.AddWithValue("@DataOraAggiornamento", aggiornamento.DataOraAggiornamento);

                    command.ExecuteNonQuery();
                }
            }
        }


        private List<AggiornamentoSpedizioneModel> GetAggiornamentiFromDatabase(int spedizioneId)
        {
            List<AggiornamentoSpedizioneModel> aggiornamenti = new List<AggiornamentoSpedizioneModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM AggiornamentiSpedizioni WHERE SpedizioneId = @SpedizioneId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SpedizioneId", spedizioneId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AggiornamentoSpedizioneModel aggiornamento = new AggiornamentoSpedizioneModel
                            {
                                Id = (int)reader["Id"],
                                SpedizioneId = (int)reader["SpedizioneId"],
                                Stato = reader["Stato"].ToString(),
                                Luogo = reader["Luogo"].ToString(),
                                Descrizione = reader["Descrizione"].ToString(),
                                DataOraAggiornamento = (DateTime)reader["DataOraAggiornamento"]
                            };

                            aggiornamenti.Add(aggiornamento);
                        }
                    }
                }
            }

            return aggiornamenti;
        }
    }
}
