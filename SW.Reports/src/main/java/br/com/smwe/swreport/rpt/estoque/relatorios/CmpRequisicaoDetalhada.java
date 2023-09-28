package br.com.smwe.swreport.rpt.estoque.relatorios;

import Base.BaseHttpServlet;
import br.com.smwe.swreport.conexao.Conexao;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.sql.Connection;
import java.util.HashMap;
import java.util.Map;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;

public class CmpRequisicaoDetalhada extends BaseHttpServlet {
    
    private static final long serialVersionUID = 1L;

    public CmpRequisicaoDetalhada() {
        super();
    }
    
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        try {
            
            // acha jrxml dentro da aplicação
            ServletContext contexto = request.getServletContext();
            String rptModelo = request.getParameter("rptModelo");
            String jrxml = this.GetJasperFile(contexto,"Estoque/Relatorios/RptCmpRequisicaoDetalhada.jasper",request.getParameter("TenancyName"));
                      
            // prepara parâmetros
            Map<String, Object> parametros = new HashMap<>();
            //Parametros do Relatório
            parametros.put("ProdutoDescricao", request.getParameter("ProdutoDescricao"));
            parametros.put("EmpresaId", request.getParameter("EmpresaId"));
            parametros.put("RequisicaoId", request.getParameter("RequisicaoId"));
            parametros.put("usuarioImpressao", request.getParameter("usuarioImpressao"));
            parametros.put("dataInicio", request.getParameter("dataInicio"));
            parametros.put("dataFinal", request.getParameter("dataFinal"));
            parametros.put("urlImagemCliente", request.getParameter("urlImagemCliente"));
            parametros.put("nomeCliente", request.getParameter("nomeCliente"));
            
            // abre conexão com o banco
            Connection conexao = Conexao.getConnection(request.getParameter("Dominio").toUpperCase());
            
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
         
            //gera relatório
            JasperPrint print = JasperFillManager.fillReport(jrxml, parametros, conexao);
            JasperExportManager.exportReportToPdfStream(print, baos);
            response.reset();
            response.setContentType("application/pdf");
            response.setHeader("Access-Control-Allow-Origin", "*");
            response.setHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
            response.setHeader("Access-Control-Allow-Credentials", "true");
            response.setHeader("Access-Control-Allow-Headers", "Content-Type, x-xsrf-token");
            response.setHeader("Content-Disposition", "inline; filename=RptCmpRequisicaoDetalhada.pdf"); 
            response.setContentLength(baos.size());
            response.getOutputStream().write(baos.toByteArray());
            response.getOutputStream().flush();
            response.getOutputStream().close();
            conexao.close(); // não esqueça de fechar a conexão
            
        } catch (Exception e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }
    
    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /**
     * Handles the HTTP <code>GET</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Handles the HTTP <code>POST</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Returns a short description of the servlet.
     *
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>
    
}
