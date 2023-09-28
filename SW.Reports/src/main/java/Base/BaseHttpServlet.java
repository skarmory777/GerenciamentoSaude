/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Base;

import java.io.File;
import javax.servlet.ServletContext;
import javax.servlet.http.HttpServlet;

/**
 *
 * @author gusta
 */
public class BaseHttpServlet extends HttpServlet {
    public String GetJasperFile(ServletContext contexto, String jasperFile,String tenancyName){
        String jrxml = contexto.getRealPath("/rpt/base/"+jasperFile);
        if(tenancyName != null){
        File f = new File(contexto.getRealPath("/rpt/"+tenancyName.toLowerCase()+"/"+jasperFile.trim()));
            if(f.isFile()){
               jrxml =  f.getAbsolutePath();
            }
        }
        return jrxml;
    }
}
