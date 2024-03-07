using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using EpiLogistic.Models;

namespace EpiLogistic.Controllers
{
    public class ClienteController : Controller
    {
        private readonly string connectionString = "Data Source=GABRIELE-PORTAT\\SQLEXPRESS;Initial Catalog=EpiLogistic;Integrated Security=True;"; 

        // GET: Cliente
        public ActionResult Index()
        {
            List<ClienteModel> clienti = GetClientiFromDatabase();
            return View(clienti);
        }

        // GET: Cliente/Details/5
        public ActionResult Details(string searchInput, bool? isAzienda)
        {
            if (!string.IsNullOrEmpty(searchInput))
            {
                // Esegue la ricerca in base al codice fiscale o partita IVA
                ClienteModel cliente = GetClienteByCodiceFiscaleOrPartitaIva(searchInput, isAzienda);

                if (cliente == null)
                {
                    return HttpNotFound();
                }

                return View(cliente);
            }

            // Se non è stata effettuata una ricerca, mostra tutti i clienti
            List<ClienteModel> clienti = GetClientiFromDatabase();
            return View(clienti);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                InsertClienteIntoDatabase(cliente);
                return RedirectToAction("Create");
            }

            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            ClienteModel cliente = GetClienteById(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                UpdateClienteInDatabase(cliente);
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            ClienteModel cliente = GetClienteById(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeleteClienteFromDatabase(id);
            return RedirectToAction("Index");
        }

        private List<ClienteModel> GetClientiFromDatabase()
        {
            List<ClienteModel> clienti = new List<ClienteModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Clienti", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClienteModel cliente = new ClienteModel
                            {
                                Id = (int)reader["Id"],
                                CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                PartitaIva = reader["PartitaIva"].ToString(),
                                IsAzienda = (bool)reader["IsAzienda"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                CAP = reader["CAP"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                            };

                            clienti.Add(cliente);
                        }
                    }
                }
            }

            return clienti;
        }

        private ClienteModel GetClienteByCodiceFiscaleOrPartitaIva(string searchInput, bool? isAzienda)
        {
            ClienteModel cliente = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Clienti WHERE CodiceFiscale = @SearchInput";

                if (isAzienda.HasValue && isAzienda.Value)
                {
                    query = "SELECT * FROM Clienti WHERE IsAzienda = 1 AND PartitaIva = @SearchInput";
                }
                else
                {
                    query += " OR PartitaIva = @SearchInput AND (IsAzienda = 0 OR IsAzienda IS NULL)";
                }

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchInput", searchInput);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new ClienteModel
                            {
                                Id = (int)reader["Id"],
                                CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                PartitaIva = reader["PartitaIva"].ToString(),
                                IsAzienda = (bool)reader["IsAzienda"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                CAP = reader["CAP"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                            };
                        }
                    }
                }
            }

            return cliente;
        }


        private ClienteModel GetClienteById(int id)
        {
            ClienteModel cliente = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Clienti WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new ClienteModel
                            {
                                Id = (int)reader["Id"],
                                CodiceFiscale = reader["CodiceFiscale"].ToString(),
                                PartitaIva = reader["PartitaIva"].ToString(),
                                IsAzienda = (bool)reader["IsAzienda"],
                                Nome = reader["Nome"].ToString(),
                                Cognome = reader["Cognome"].ToString(),
                                Citta = reader["Citta"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                CAP = reader["CAP"].ToString(),
                                Email = reader["Email"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                            };
                        }
                    }
                }
            }

            return cliente;
        }

        private void InsertClienteIntoDatabase(ClienteModel cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO Clienti (CodiceFiscale, PartitaIva, IsAzienda, Nome, Cognome, Citta, Indirizzo, CAP, Email, Telefono) " +
                    "VALUES (@CodiceFiscale, @PartitaIva, @IsAzienda, @Nome, @Cognome, @Citta, @Indirizzo, @CAP, @Email, @Telefono)", connection))
                {
                    command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                    command.Parameters.AddWithValue("@PartitaIva", cliente.PartitaIva);
                    command.Parameters.AddWithValue("@IsAzienda", cliente.IsAzienda);
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                    command.Parameters.AddWithValue("@Citta", cliente.Citta);
                    command.Parameters.AddWithValue("@Indirizzo", cliente.Indirizzo);
                    command.Parameters.AddWithValue("@CAP", cliente.CAP);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateClienteInDatabase(ClienteModel cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "UPDATE Clienti SET CodiceFiscale = @CodiceFiscale, PartitaIva = @PartitaIva, " +
                    "IsAzienda = @IsAzienda, Nome = @Nome, Cognome = @Cognome, Citta = @Citta, " +
                    "Indirizzo = @Indirizzo, CAP = @CAP, Email = @Email, Telefono = @Telefono " +
                    "WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                    command.Parameters.AddWithValue("@PartitaIva", cliente.PartitaIva);
                    command.Parameters.AddWithValue("@IsAzienda", cliente.IsAzienda);
                    command.Parameters.AddWithValue("@Nome", cliente.Nome);
                    command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                    command.Parameters.AddWithValue("@Citta", cliente.Citta);
                    command.Parameters.AddWithValue("@Indirizzo", cliente.Indirizzo);
                    command.Parameters.AddWithValue("@CAP", cliente.CAP);
                    command.Parameters.AddWithValue("@Email", cliente.Email);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    command.Parameters.AddWithValue("@Id", cliente.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteClienteFromDatabase(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM Clienti WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
