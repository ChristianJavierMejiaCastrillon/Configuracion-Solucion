using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Linq.Expressions;



namespace negocio
{
    public class PokemonNegocio
    {
        public List<Pokemon> listar()
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pokemon auxiliar = new Pokemon();
                    auxiliar.Id = (int)datos.Lector["Id"];
                    auxiliar.Numero = (int)datos.Lector.GetInt32(0);
                    auxiliar.Nombre = (string)datos.Lector["Nombre"];
                    auxiliar.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                    auxiliar.UrlImagen = (string)datos.Lector["UrlImagen"];

                    auxiliar.Tipo = new Elemento();
                    auxiliar.Tipo.Id = (int)datos.Lector["IdTipo"];
                    auxiliar.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    auxiliar.Debilidad = new Elemento();
                    auxiliar.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    auxiliar.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(auxiliar);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConcexion();
            }
        }

        public void agregar(Pokemon nuevo) 
        {
            AccesoDatos accesoDatos = new AccesoDatos();

            try
            {
                accesoDatos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen) values (" + nuevo.Numero + ", '" + nuevo.Nombre +"' , '" + nuevo.Descripcion +"', 1, @idTipo, @idDebilidad, @urlImagen)");
                accesoDatos.setearParametro("@idTipo", nuevo.Tipo.Id);
                accesoDatos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
                accesoDatos.setearParametro("@urlImagen", nuevo.UrlImagen);
                accesoDatos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConcexion();
            }
        }

        public void modificar(Pokemon poke) 
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @descripcion, UrlImagen = @imagen, IdTipo = @idtipo, IdDebilidad = @iddebilidad where Id =@id");
                datos.setearParametro("@numero", poke.Numero);
                datos.setearParametro("@nombre", poke.Nombre);
                datos.setearParametro("@descripcion", poke.Descripcion);
                datos.setearParametro("@imagen", poke.UrlImagen);
                datos.setearParametro("@idtipo", poke.Tipo.Id);
                datos.setearParametro("@iddebilidad", poke.Debilidad.Id);
                datos.setearParametro("@id", poke.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                datos.cerrarConcexion();
            }
        }

        public void eliminarfisico(int id) 
        {
            try
            {
                AccesoDatos accesoDatos = new AccesoDatos();
                accesoDatos.setearConsulta("delete from POKEMONS where id = @id");
                accesoDatos.setearParametro("@id", id);
                accesoDatos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void eliminarLogico(int id)
        {
            try
            {
                AccesoDatos accesoDatos = new AccesoDatos();
                accesoDatos.setearConsulta("update POKEMONS set Activo = 0  where id = @id");
                accesoDatos.setearParametro("@id", id);
                accesoDatos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1 And ";
                if (campo == "Numero")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "P.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "P.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "P.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Pokemon auxiliar = new Pokemon();
                    auxiliar.Id = (int)datos.Lector["Id"];
                    auxiliar.Numero = (int)datos.Lector.GetInt32(0);
                    auxiliar.Nombre = (string)datos.Lector["Nombre"];
                    auxiliar.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        auxiliar.UrlImagen = (string)datos.Lector["UrlImagen"];

                    auxiliar.Tipo = new Elemento();
                    auxiliar.Tipo.Id = (int)datos.Lector["IdTipo"];
                    auxiliar.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    auxiliar.Debilidad = new Elemento();
                    auxiliar.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    auxiliar.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    lista.Add(auxiliar);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
    