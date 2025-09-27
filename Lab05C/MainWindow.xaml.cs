using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab05C
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool modoEdicion = false;
        private string clienteIdOriginal = "";
        private bool enBusqueda = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnListarClientes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clientes = Cliente.ListarClientes();
                dgClientes.ItemsSource = clientes;

                // Cambiar visibilidad
                welcomePanel.Visibility = Visibility.Collapsed;
                formScrollViewer.Visibility = Visibility.Collapsed;
                gridListaClientes.Visibility = Visibility.Visible;

                // Actualizar estado
                enBusqueda = false;
                lblResultados.Text = $"Mostrando todos los clientes ({clientes.Count} registros)";

                MessageBox.Show($"Se cargaron {clientes.Count} clientes", "Información",
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            RealizarBusqueda();
        }

        private void TxtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RealizarBusqueda();
            }
        }

        private void TxtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Búsqueda automática mientras escribe (opcional)
            if (string.IsNullOrWhiteSpace(txtBuscar.Text) && enBusqueda)
            {
                // Si borra todo el texto, mostrar todos los clientes
                BtnListarClientes_Click(sender, new RoutedEventArgs());
            }
        }

        private void BtnLimpiarBusqueda_Click(object sender, RoutedEventArgs e)
        {
            txtBuscar.Clear();
            if (enBusqueda)
            {
                BtnListarClientes_Click(sender, new RoutedEventArgs());
            }
            txtBuscar.Focus();
        }

        private void RealizarBusqueda()
        {
            string terminoBusqueda = txtBuscar.Text.Trim();

            if (string.IsNullOrWhiteSpace(terminoBusqueda))
            {
                MessageBox.Show("⚠️ Por favor ingrese un término de búsqueda", "Búsqueda vacía",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtBuscar.Focus();
                return;
            }

            try
            {
                var clientesEncontrados = Cliente.BuscarClientes(terminoBusqueda);
                dgClientes.ItemsSource = clientesEncontrados;

                // Cambiar visibilidad
                welcomePanel.Visibility = Visibility.Collapsed;
                formScrollViewer.Visibility = Visibility.Collapsed;
                gridListaClientes.Visibility = Visibility.Visible;

                // Actualizar estado
                enBusqueda = true;
                lblResultados.Text = $"Resultados de búsqueda para '{terminoBusqueda}': {clientesEncontrados.Count} cliente(s) encontrado(s)";

                if (clientesEncontrados.Count == 0)
                {
                    MessageBox.Show($"🔍 No se encontraron clientes con el término '{terminoBusqueda}'", "Sin resultados",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"✅ Se encontraron {clientesEncontrados.Count} cliente(s)", "Búsqueda exitosa",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error en la búsqueda: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRegistrarCliente_Click(object sender, RoutedEventArgs e)
        {
            // Configurar modo registro
            modoEdicion = false;
            ConfigurarModoFormulario();

            // Cambiar visibilidad
            welcomePanel.Visibility = Visibility.Collapsed;
            gridListaClientes.Visibility = Visibility.Collapsed;
            formScrollViewer.Visibility = Visibility.Visible;

            LimpiarCampos();
            txtIdCliente.Focus();
        }

        private void BtnEditarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is Cliente clienteSeleccionado)
            {
                EditarCliente(clienteSeleccionado);
            }
            else
            {
                MessageBox.Show("⚠️ Por favor seleccione un cliente de la lista para editar",
                               "Seleccionar Cliente",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is Cliente clienteSeleccionado)
            {
                MessageBoxResult resultado = MessageBox.Show(
                    $"⚠️ ¿Está seguro que desea eliminar el cliente?\n\n" +
                    $"ID: {clienteSeleccionado.IdCliente}\n" +
                    $"Compañía: {clienteSeleccionado.NombreCompañia}\n" +
                    $"Contacto: {clienteSeleccionado.NombreContacto}\n\n" +
                    $"Esta acción no se puede deshacer.",
                    "Confirmar Eliminación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (Cliente.EliminarCliente(clienteSeleccionado.IdCliente))
                        {
                            MessageBox.Show("✅ Cliente eliminado exitosamente", "Éxito",
                                           MessageBoxButton.OK, MessageBoxImage.Information);

                            // Recargar la lista según el estado actual
                            if (enBusqueda && !string.IsNullOrWhiteSpace(txtBuscar.Text))
                            {
                                RealizarBusqueda();
                            }
                            else
                            {
                                BtnListarClientes_Click(sender, e);
                            }
                        }
                        else
                        {
                            MessageBox.Show("❌ No se pudo eliminar el cliente", "Error",
                                           MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al eliminar cliente: {ex.Message}", "Error",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("⚠️ Por favor seleccione un cliente de la lista para eliminar",
                               "Seleccionar Cliente",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cliente = new Cliente
                {
                    IdCliente = txtIdCliente.Text.Trim(),
                    NombreCompañia = txtNombreCompañia.Text.Trim(),
                    NombreContacto = txtNombreContacto.Text.Trim(),
                    CargoContacto = txtCargoContacto.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Ciudad = txtCiudad.Text.Trim(),
                    Region = txtRegion.Text.Trim(),
                    CodPostal = txtCodPostal.Text.Trim(),
                    Pais = txtPais.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Fax = txtFax.Text.Trim()
                };

                if (Cliente.ValidarCliente(cliente))
                {
                    if (!Cliente.ExisteCliente(cliente.IdCliente))
                    {
                        MessageBoxResult resultado = MessageBox.Show(
                            $"¿Está seguro que desea guardar el cliente '{cliente.NombreCompañia}'?",
                            "Confirmar Registro",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (resultado == MessageBoxResult.Yes)
                        {
                            if (Cliente.InsertarCliente(cliente))
                            {
                                MessageBox.Show("✅ Cliente guardado exitosamente", "Éxito",
                                               MessageBoxButton.OK, MessageBoxImage.Information);

                                LimpiarCampos();

                                MessageBoxResult registrarOtro = MessageBox.Show(
                                    "¿Desea registrar otro cliente?",
                                    "Continuar Registro",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question);

                                if (registrarOtro == MessageBoxResult.No)
                                {
                                    MostrarPanelBienvenida();
                                }
                                else
                                {
                                    txtIdCliente.Focus();
                                }
                            }
                            else
                            {
                                MessageBox.Show("❌ No se pudo guardar el cliente", "Error",
                                               MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("⚠️ Ya existe un cliente con ese ID.\nPor favor ingrese un ID diferente.",
                                       "Cliente Duplicado",
                                       MessageBoxButton.OK, MessageBoxImage.Warning);

                        txtIdCliente.SelectAll();
                        txtIdCliente.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("⚠️ Por favor complete los campos obligatorios:\n• ID Cliente\n• Nombre de Compañía\n• Nombre de Contacto",
                                   "Campos Requeridos",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);

                    EnfocarPrimerCampoVacio();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar cliente: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cliente = new Cliente
                {
                    IdCliente = txtIdCliente.Text.Trim(),
                    NombreCompañia = txtNombreCompañia.Text.Trim(),
                    NombreContacto = txtNombreContacto.Text.Trim(),
                    CargoContacto = txtCargoContacto.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Ciudad = txtCiudad.Text.Trim(),
                    Region = txtRegion.Text.Trim(),
                    CodPostal = txtCodPostal.Text.Trim(),
                    Pais = txtPais.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Fax = txtFax.Text.Trim()
                };

                if (Cliente.ValidarCliente(cliente))
                {
                    MessageBoxResult resultado = MessageBox.Show(
                        $"¿Está seguro que desea actualizar el cliente '{cliente.NombreCompañia}'?",
                        "Confirmar Actualización",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (resultado == MessageBoxResult.Yes)
                    {
                        if (Cliente.ActualizarCliente(cliente))
                        {
                            MessageBox.Show("✅ Cliente actualizado exitosamente", "Éxito",
                                           MessageBoxButton.OK, MessageBoxImage.Information);

                            // Recargar la lista según el estado actual
                            if (enBusqueda && !string.IsNullOrWhiteSpace(txtBuscar.Text))
                            {
                                RealizarBusqueda();
                            }
                            else
                            {
                                BtnListarClientes_Click(sender, e);
                            }
                        }
                        else
                        {
                            MessageBox.Show("❌ No se pudo actualizar el cliente", "Error",
                                           MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("⚠️ Por favor complete los campos obligatorios:\n• ID Cliente\n• Nombre de Compañía\n• Nombre de Contacto",
                                   "Campos Requeridos",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);

                    EnfocarPrimerCampoVacio();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al actualizar cliente: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();

            MessageBox.Show("Campos limpiados correctamente", "Información",
                           MessageBoxButton.OK, MessageBoxImage.Information);

            txtIdCliente.Focus();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            MostrarPanelBienvenida();
        }

        private void DgClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgClientes.SelectedItem is Cliente clienteSeleccionado)
            {
                EditarCliente(clienteSeleccionado);
            }
        }

        private void EditarCliente(Cliente cliente)
        {
            // Configurar modo edición
            modoEdicion = true;
            clienteIdOriginal = cliente.IdCliente;
            ConfigurarModoFormulario();

            // Llenar campos con datos del cliente
            txtIdCliente.Text = cliente.IdCliente;
            txtNombreCompañia.Text = cliente.NombreCompañia;
            txtNombreContacto.Text = cliente.NombreContacto;
            txtCargoContacto.Text = cliente.CargoContacto;
            txtDireccion.Text = cliente.Direccion;
            txtCiudad.Text = cliente.Ciudad;
            txtRegion.Text = cliente.Region;
            txtCodPostal.Text = cliente.CodPostal;
            txtPais.Text = cliente.Pais;
            txtTelefono.Text = cliente.Telefono;
            txtFax.Text = cliente.Fax;

            // Deshabilitar edición del ID
            txtIdCliente.IsEnabled = false;

            // Cambiar visibilidad
            gridListaClientes.Visibility = Visibility.Collapsed;
            welcomePanel.Visibility = Visibility.Collapsed;
            formScrollViewer.Visibility = Visibility.Visible;

            txtNombreCompañia.Focus();
        }

        private void ConfigurarModoFormulario()
        {
            if (modoEdicion)
            {
                lblTituloFormulario.Text = "EDITAR CLIENTE";
                btnGuardar.Visibility = Visibility.Collapsed;
                btnActualizar.Visibility = Visibility.Visible;
                txtIdCliente.IsEnabled = false;
            }
            else
            {
                lblTituloFormulario.Text = "REGISTRAR NUEVO CLIENTE";
                btnGuardar.Visibility = Visibility.Visible;
                btnActualizar.Visibility = Visibility.Collapsed;
                txtIdCliente.IsEnabled = true;
            }
        }

        private void MostrarPanelBienvenida()
        {
            // Resetear modo
            modoEdicion = false;
            txtIdCliente.IsEnabled = true;

            // Cambiar visibilidad
            formScrollViewer.Visibility = Visibility.Collapsed;
            gridListaClientes.Visibility = Visibility.Collapsed;
            welcomePanel.Visibility = Visibility.Visible;

            LimpiarCampos();
        }

        private void EnfocarPrimerCampoVacio()
        {
            if (string.IsNullOrWhiteSpace(txtIdCliente.Text))
                txtIdCliente.Focus();
            else if (string.IsNullOrWhiteSpace(txtNombreCompañia.Text))
                txtNombreCompañia.Focus();
            else if (string.IsNullOrWhiteSpace(txtNombreContacto.Text))
                txtNombreContacto.Focus();
        }

        private void LimpiarCampos()
        {
            txtIdCliente.Clear();
            txtNombreCompañia.Clear();
            txtNombreContacto.Clear();
            txtCargoContacto.Clear();
            txtDireccion.Clear();
            txtCiudad.Clear();
            txtRegion.Clear();
            txtCodPostal.Clear();
            txtPais.Clear();
            txtTelefono.Clear();
            txtFax.Clear();
        }
    }
}