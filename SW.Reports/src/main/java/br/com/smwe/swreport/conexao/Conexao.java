package br.com.smwe.swreport.conexao;

import java.sql.Connection;
import java.sql.SQLException;
import javax.naming.InitialContext;
import javax.sql.DataSource;
/**
 *
 * @author SMWE
 */
public class Conexao {
    
    public static Connection getConnection(String Dominio) throws Exception{
        InitialContext context = new InitialContext(); 
        DataSource ds = (DataSource)context.lookup("java:comp/env/"+Dominio);
        try {
            return ds.getConnection();
        } catch (SQLException e) {
            throw new Exception(e.getMessage());
        }
    }
    
}
