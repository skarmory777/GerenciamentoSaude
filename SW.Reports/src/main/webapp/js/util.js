/*

Classe para manipulacao de datas
@author Wallas dos Santos Souza
@since 28 de Dezembro de 2007

@formato: String
'brasileiro': formata a data de saida no formato dd/mm/aaaa
'americano': formata a data de saida no formato aaaa/mm/dd

@qtdSemanas: int
informe quantas semanas deseja resgatar.

@semanal: String
'inicio_fim': formata a data de saida no formato <inicio da semana>,<fim da semana>
'fim_inicio': formata a data de saida no formato <fim da semana>,<inicio da semana>

*/

function Datas(formato,qtdSemanas,semanal){

 this.retorno  = iniciar(formato,qtdSemanas,semanal);
  
 function iniciar(formato,qtdsSemanas,semanal){
 
    data = new Date();
    dia = data.getDate();
    mes = data.getMonth();
    ano = data.getFullYear();

    if(mes == 0)
       mes_ex = "January";
    else if (mes == 1)
       mes_ex = "February";
    else if (mes == 2)
       mes_ex = "March"
    else if (mes == 3)
       mes_ex = "April"
    else if (mes == 4)
       mes_ex = "May"
    else if (mes == 5)
       mes_ex = "June"
    else if (mes == 6)
       mes_ex = "July"
    else if (mes == 7)
       mes_ex = "August"
    else if (mes == 8)
       mes_ex = "September"
    else if (mes == 9)
       mes_ex = "October"
    else if (mes == 10)
       mes_ex = "November"
    else if (mes == 11)
       mes_ex = "December"            

    dia_ex = dia + " " + mes_ex + " " + ano;

    return retornaSemanal(dia_ex,formato,qtdsSemanas,semanal);
 }
   
 function retornaSemanal(dia_ex,formato,semanas,semanal){
  		
    um_dia = 1000
    uma_semana = 24*60*60*(um_dia*7)
    seis_dias = 24*60*60*(um_dia*6)

    contagem = fazContagem(data.getDay());

    inicial = "";
    inicio_da_semana = new Array(semanas);
    fim_da_semana = new Array(semanas);

    //semana atual  
    d = new Date (dia_ex)

    d.setTime (d.getTime() - contagem);

    if (formato == 'brasileiro')
            inicio_da_semana[0] = d.getDate() + "/" + (d.getMonth()+1) + "/" + d.getFullYear();
    else if (formato == 'americano')
            inicio_da_semana[0] = d.getFullYear() + "/" + (d.getMonth()+1) + "/" + d.getDate();

    inicial = d.getTime() - 24*60*60*1000;
		
    d.setTime (d.getTime () + seis_dias)
    if (formato == 'brasileiro')
            fim_da_semana[0] = d.getDate() + "/" + (d.getMonth()+1) + "/" + d.getFullYear();
    else if (formato == 'americano')
            fim_da_semana[0] = d.getFullYear() + "/" + (d.getMonth()+1) + "/" + d.getDate();

    //agora que ja tenho o inicio e fim da primeira semana posso calcular as anteriores	

    //demais semanas
    for(i=1; i<(semanas); i++){
        d = new Date (inicial)

        if (formato == 'brasileiro')
                fim_da_semana[i] = d.getDate() + "/" + (d.getMonth()+1) + "/" + d.getFullYear();
        else if (formato == 'americano')
                fim_da_semana[i] = d.getFullYear() + "/" + (d.getMonth()+1) + "/" + d.getDate();

        d.setTime (d.getTime () - seis_dias)
        if (formato == 'brasileiro')
                inicio_da_semana[i] = d.getDate() + "/" + (d.getMonth()+1) + "/" + d.getFullYear();
        else if (formato == 'americano')
                inicio_da_semana[i] = d.getFullYear() + "/" + (d.getMonth()+1) + "/" + d.getDate();

        inicial = d.getTime() - 24*60*60*1000;
    }		
	
    fim = new Array (semanas);
		
    for(j = 0; j<fim.length; j++){
        if (semanal == 'inicio_fim')
            fim[j] = fim_da_semana[j] + "," + inicio_da_semana[j];
        else if(semanal == 'fim_inicio')
            fim[j] = inicio_da_semana[j] + "," + fim_da_semana[j];
    }

    return fim;
  }

  function fazContagem(dia_semana){
    for(i=0; i<dia_semana; i++){
        if(dia_semana == 0)
            return 0;
        else
            return 24*60*60*(1000*dia_semana);
    }
  }

}


