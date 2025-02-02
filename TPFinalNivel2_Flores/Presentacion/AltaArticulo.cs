using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class AltaArticulo : Form
    {
        private Articulo Articulo = null;

        public AltaArticulo()
        {
            InitializeComponent();
            Text = "Nuevo Articulo";
        }

        public AltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.Articulo = articulo;
            Text = "Modificar Articulo";
        }


        private void AltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            try
            {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                if(Articulo != null)
                {
                    txtCodArticulo.Text = Articulo.Codigo.ToString();
                    txtNombre.Text = Articulo.Nombre;
                    txtDescripcion.Text = Articulo.Descripcion;
                    txtImagenUrl.Text = Articulo.ImagenUrl;
                    cargarImagen(Articulo.ImagenUrl);
                    cboCategoria.SelectedValue = Articulo.Categoria.Id;
                    cboMarca.SelectedValue = Articulo.Marca.Id;
                    txtPrecio.Text = Articulo.Precio.ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }


        private bool validarAgregado()
        {
            if (string.IsNullOrEmpty(txtCodArticulo.Text))
            {
                MessageBox.Show("Se debe ingresar un codigo de articulo..");
                return true;
            }
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Se debe ingresar un nombre de articulo..");
                return true;
            }
            if (!esDecimalValido(txtPrecio.Text))
            {
                MessageBox.Show("Debe ingresar solo numeros en el precio..");
            }



            return false;
        }


        private bool esDecimalValido(string texto)
        {
            decimal resultado;
            return decimal.TryParse(texto, out resultado);
        }

       

        
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio articulo = new ArticuloNegocio();

            try
            {
                if(Articulo == null)
                    Articulo = new Articulo();

                if(validarAgregado())
                    return;

                Articulo.Codigo = (txtCodArticulo.Text);
                Articulo.Nombre = (txtNombre.Text);
                Articulo.Descripcion = (txtDescripcion.Text);
                Articulo.ImagenUrl = (txtImagenUrl.Text);
                Articulo.Marca = (Marca)cboMarca.SelectedItem;
                Articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                Articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (Articulo.Id != 0)
                {
                    articulo.modificar(Articulo);
                    MessageBox.Show("Modificado Exitosamente");
                }
                else
                {
                    articulo.agregar(Articulo);
                    MessageBox.Show("Agregado Exitosamente");
                }
                
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);

            }
            catch (Exception)
            {
                pbxImagen.Load("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=");
            }
        }


        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
        }



    }
}
