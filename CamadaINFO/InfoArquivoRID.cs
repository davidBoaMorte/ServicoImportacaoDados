using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamadaINFO
{
    public class InfoArquivoRID
    {
        // CARTA PADRÃO CORPO
        public string DiretorioCartaGerada { get; set; }
        public string NomeCarta { get; set; }
        public string DVG_TEXTO_0011 { get; set; }
        public string DVG_TEXTO_0012 { get; set; }
        public string DVG_TEXTO_0013 { get; set; }
        public string DVG_TEXTO_0015 { get; set; }
        public string DVG_TEXTO_0017 { get; set; }
        public string DVG_TEXTO_0019 { get; set; }
        public string DVG_TEXTO_0022 { get; set; }
        public string DVG_TEXTO_0024 { get; set; }
        public string DVG_TEXTO_0025 { get; set; }
        public string DVG_TEXTO_0026 { get; set; }
        public string DVG_TEXTO_0028 { get; set; }
        public string DVG_TEXTO_0029 { get; set; }
        public string DVG_TEXTO_0031 { get; set; }
        public string DVG_TEXTO_0032 { get; set; }
        public string DVG_TEXTO_0033 { get; set; }
        public string DVG_TEXTO_0034 { get; set; }
        public string DVG_TEXTO_0035 { get; set; }
        public string DVG_TEXTO_0036 { get; set; }
        public string DVG_TEXTO_0037 { get; set; }
        public string DVG_TEXTO_0038 { get; set; }
        public string DVG_TEXTO_0039 { get; set; }
        public string DVG_TEXTO_0040 { get; set; }
        public string DVG_TEXTO_0041 { get; set; }
        public string DVG_TEXTO_0043 { get; set; }
        public string DVG_TEXTO_0044 { get; set; }
        public string DVG_TEXTO_0046 { get; set; }
        public string DVG_TEXTO_0047 { get; set; }
        public string CRDI_CONTROL_BEGIN { get; set; }
        public string CRDI_CONTROL_END { get; set; }

        //TOPO DA CARTA
        /// <summary>
        /// Nome do Topo da Carta
        /// </summary>
        public string DGV_NAME_TOPO { get; set; }
        /// <summary>
        /// Endereço do Topo da Carta
        /// </summary>
        public string DY_ADDR_LINES_LINE1 { get; set; }
        /// <summary>
        /// Bairro do Topo da Carta
        /// </summary>
        public string DY_ADDR_LINES_LINE2 {get; set; }
        /// <summary>
        /// CEP / Município / Estado do Topo da Carta
        /// </summary>
        public string DY_ADDR_LINES_LINE3 { get; set; }

        //VERSO DA CARTA
        /// <summary>
        /// Nome do Verso da Carta
        /// </summary>
        public string DGV_NAME_VERSO { get; set; }
        /// <summary>
        /// Endereço do Verso da Carta
        /// </summary>
        public string DGV_END_ENT1 { get; set; }
        /// <summary>
        /// Bairro do Verso da Carta
        /// </summary>
        public string DGV_END_ENT2 { get; set; }
        /// <summary>
        /// CEP / Município / Estado do Verso da Carta
        /// </summary>
        public string DGV_END_ENT3 { get; set; }
        /// <summary>
        /// Instalação do Verso da Carta
        /// </summary>
        public string DGV_INSTAL { get; set; }
        /// <summary>
        /// Rota de Entrada do Verso da Carta.
        /// </summary>
        public string DGV_ROT_ENTREGA { get; set; }
        public int ErroProcessamento { get; set; }
    }
}
