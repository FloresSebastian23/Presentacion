using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccesoDatos;
using Dominio;
using Negocio;

namespace Presentacion
{
    public partial class FrmArticulos : Form
    {
        private List<Articulo> listaArticulo;

        public FrmArticulos()
        {
            InitializeComponent();
        }


        private void FrmArticulos_Load(object sender, EventArgs e)
        {
            cargar();
        }


        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            listaArticulo = negocio.listar();
            dgvArticulos.DataSource = listaArticulo;
            ocultarColomnas();
            cargarImagen(listaArticulo[0].ImagenUrl);

        }


        private void ocultarColomnas()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
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


        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulo alta = new AltaArticulo();
            alta.ShowDialog();
            cargar();
        }


        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> filtroArticulos;
            string filtro = txtFiltroRapido.Text;

            if(filtro != "")
            {
                filtroArticulos = listaArticulo.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                filtroArticulos = listaArticulo;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = filtroArticulos;
            ocultarColomnas();
            verificarGrilla();
        }

        private void verificarGrilla()
        {
            if (dgvArticulos.SelectedRows.Count == 0)
            {
                btnEliminarFisico.Enabled = false;
                btnModificar.Enabled = false;
            }
            else
            {
                btnEliminarFisico.Enabled= true;
                btnModificar.Enabled= true;
            }

            }


        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            AltaArticulo modificar = new AltaArticulo(seleccionado);
            modificar.ShowDialog();
            cargar();
        }


        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿Seguro desea eliminar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                    negocio.eliminar(seleccionado.Id);

                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
