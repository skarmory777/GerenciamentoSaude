/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package br.com.smwe.swreport.rpt.estoque;

import Base.BaseHttpServlet;
import br.com.smwe.swreport.conexao.Conexao;
//import br.com.bgb.daniellipprpt.core.GeradordeRelatorios;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.sql.Connection;
import java.util.HashMap;
import java.util.Map;
import javax.servlet.ServletContext;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;

/**
 *
 * @author SMWE
 */

public class EtiquetaLoteValidade extends BaseHttpServlet {
    
    private static final long serialVersionUID = 1L;

    public EtiquetaLoteValidade() {
        super();
        // TODO Auto-generated constructor stub
    }
    /**
     * Processes requests for both HTTP <code>GET</code> and <code>POST</code>
     * methods.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        try {
            
            // acha jrxml dentro da aplicação
            ServletContext contexto = request.getServletContext();
            String rptModelo = request.getParameter("rptModelo");
            
                      
            // prepara parâmetros
            Map<String, Object> parametros = new HashMap<>();
            Integer modelo = Integer.parseInt(request.getParameter("Modelo"));
            String jrxml = this.GetJasperFile(contexto,"Estoque/Etiquetas_modelo_"+modelo+".jasper",request.getParameter("TenancyName"));
            
            
            Double qtd = Double.parseDouble(request.getParameter("Qtd"));
            
            //Parametros do Relatório
            parametros.put("LoteValidadeId", Long.parseLong(request.getParameter("LoteValidadeId")));
            parametros.put("Qtd", (int) Math.ceil(qtd/modelo));
            parametros.put("DataFracionamento", request.getParameter("DataFracionamento"));
            
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
            response.setHeader("Access-Control-Allow-Credentials","true");
            response.setHeader("Access-Control-Allow-Headers", "Content-Type, x-xsrf-token");
            response.setHeader("Content-Disposition", "inline; filename=ContaMedica_"+request.getParameter("contamedicaid")+".pdf"); 
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
