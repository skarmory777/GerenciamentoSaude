É
{C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\ConsultaNaoEncerradosMDFe\ServicoMDFeConsultaNaoEncerrados.cs
	namespace&& 	
MDFe&&
 
.&& 
Servicos&& 
.&& %
ConsultaNaoEncerradosMDFe&& 1
{'' 
public(( 

class(( ,
 ServicoMDFeConsultaNaoEncerrados(( 1
{)) 
public** 
MDFeRetConsMDFeNao** !%
MDFeConsultaNaoEncerrados**" ;
(**; <
string**< B
cnpj**C G
)**G H
{++ 	
var,, 
consMDFeNaoEnc,, 
=,,  
ClassesFactory,,! /
.,,/ 0
CriarConsMDFeNaoEnc,,0 C
(,,C D
cnpj,,D H
),,H I
;,,I J
consMDFeNaoEnc-- 
.-- 
ValidarSchema-- (
(--( )
)--) *
;--* +
consMDFeNaoEnc.. 
... 
SalvarXmlEmDisco.. +
(..+ ,
).., -
;..- .
var00 

webService00 
=00 
WsdlFactory00 (
.00( )"
CriaWsdlMDFeConsNaoEnc00) ?
(00? @
)00@ A
;00A B
var11 

retornoXml11 
=11 

webService11 '
.11' (
mdfeConsNaoEnc11( 6
(116 7
consMDFeNaoEnc117 E
.11E F
CriaRequestWs11F S
(11S T
)11T U
)11U V
;11V W
var33 
retorno33 
=33 
MDFeRetConsMDFeNao33 ,
.33, -
LoadXmlString33- :
(33: ;

retornoXml33; E
.33E F
OuterXml33F N
,33N O
consMDFeNaoEnc33P ^
)33^ _
;33_ `
retorno44 
.44 
SalvarXmlEmDisco44 $
(44$ %
cnpj44% )
)44) *
;44* +
return66 
retorno66 
;66 
}77 	
}88 
}99 Ÿ
sC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\ConsultaProtocoloMDFe\ServicoMDFeConsultaProtocolo.cs
	namespace&& 	
MDFe&&
 
.&& 
Servicos&& 
.&& !
ConsultaProtocoloMDFe&& -
{'' 
public(( 

class(( (
ServicoMDFeConsultaProtocolo(( -
{)) 
public** 
MDFeRetConsSitMDFe** !!
MDFeConsultaProtocolo**" 7
(**7 8
string**8 >
chave**? D
)**D E
{++ 	
var,, 
consSitMdfe,, 
=,, 
ClassesFactory,, ,
.,,, -
CriarConsSitMDFe,,- =
(,,= >
chave,,> C
),,C D
;,,D E
consSitMdfe-- 
.-- 
ValidarSchema-- %
(--% &
)--& '
;--' (
consSitMdfe.. 
... 
SalvarXmlEmDisco.. (
(..( )
)..) *
;..* +
var00 

webService00 
=00 
WsdlFactory00 (
.00( ) 
CriaWsdlMDFeConsulta00) =
(00= >
)00> ?
;00? @
var11 

retornoXml11 
=11 

webService11 '
.11' (
mdfeConsultaMDF11( 7
(117 8
consSitMdfe118 C
.11C D
CriaRequestWs11D Q
(11Q R
)11R S
)11S T
;11T U
var33 
retorno33 
=33 
MDFeRetConsSitMDFe33 ,
.33, -
LoadXml33- 4
(334 5

retornoXml335 ?
.33? @
OuterXml33@ H
,33H I
consSitMdfe33J U
)33U V
;33V W
retorno44 
.44 
SalvarXmlEmDisco44 $
(44$ %
chave44% *
)44* +
;44+ ,
return66 
retorno66 
;66 
}77 	
}88 
}99 ¡
[C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\Enderecos\Helper\UrlHelper.cs
	namespace$$ 	
MDFe$$
 
.$$ 
Servicos$$ 
.$$ 
	Enderecos$$ !
.$$! "
Helper$$" (
{%% 
public&& 

static&& 
class&& 
	UrlHelper&& !
{'' 
public(( 
static(( 
UrlMDFe(( 
ObterUrlServico(( -
(((- .
TipoAmbiente((. :
ambiente((; C
)((C D
{)) 	
switch** 
(** 
ambiente** 
)** 
{++ 
case,, 
TipoAmbiente,, !
.,,! "
Producao,," *
:,,* +
return-- 
new-- 
UrlMDFe-- &
{.. 
MDFeConsNaoEnc// &
=//' (
$str//) h
,//h i
MDFeStatusServico00 )
=00* +
$str00, u
,00u v
MDFeRetRecepcao11 '
=11( )
$str11* o
,11o p
MDFeConsulta22 $
=22% &
$str22' f
,22f g
MDFeRecepcaoEvento33 *
=33+ ,
$str33- x
,33x y
MDFeRecepcao44 $
=44% &
$str44' f
}55 
;55 
case66 
TipoAmbiente66 !
.66! "
Homologacao66" -
:66- .
return77 
new77 
UrlMDFe77 &
{88 
MDFeConsNaoEnc99 &
=99' (
$str99) x
,99x y
MDFeConsulta:: $
=::% &
$str::' r
,::r s
MDFeRecepcao;; $
=;;% &
$str;;' r
,;;r s
MDFeRecepcaoEvento<< *
=<<+ ,
$str	<<- Ñ
,
<<Ñ Ö
MDFeRetRecepcao== '
===( )
$str==* {
,=={ |
MDFeStatusServico>> )
=>>* +
$str	>>, Å
}?? 
;?? 
}@@ 
throwBB 
newBB %
InvalidOperationExceptionBB /
(BB/ 0
$strBB0 K
)BBK L
;BBL M
}CC 	
}DD 
}EE ä

RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\Enderecos\UrlMDFe.cs
	namespace!! 	
MDFe!!
 
.!! 
Servicos!! 
.!! 
	Enderecos!! !
{"" 
public## 

class## 
UrlMDFe## 
{$$ 
public%% 
string%% 
MDFeRecepcao%% "
{%%# $
get%%% (
;%%( )
set%%* -
;%%- .
}%%/ 0
public&& 
string&& 
MDFeRetRecepcao&& %
{&&& '
get&&( +
;&&+ ,
set&&- 0
;&&0 1
}&&2 3
public'' 
string'' 
MDFeRecepcaoEvento'' (
{'') *
get''+ .
;''. /
set''0 3
;''3 4
}''5 6
public(( 
string(( 
MDFeConsulta(( "
{((# $
get((% (
;((( )
set((* -
;((- .
}((/ 0
public)) 
string)) 
MDFeStatusServico)) '
{))( )
get))* -
;))- .
set))/ 2
;))2 3
}))4 5
public** 
string** 
MDFeConsNaoEnc** $
{**% &
get**' *
;*** +
set**, /
;**/ 0
}**1 2
}++ 
},, Ì
iC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\EventosMDFe\Contratos\IServicoController.cs
	namespace&& 	
MDFe&&
 
.&& 
Servicos&& 
.&& 
EventosMDFe&& #
.&&# $
	Contratos&&$ -
{'' 
public(( 

	interface(( 
IServicoController(( '
{)) 
MDFeRetEventoMDFe** 
Executar** "
(**" #
MDFeEletronico**# 1
mdfe**2 6
,**6 7
byte**8 <
sequenciaEvento**= L
,**L M
MDFeEventoContainer**N a
eventoContainer**b q
,**q r
MDFeTipoEvento	**s Å

tipoEvento
**Ç å
)
**å ç
;
**ç é
}++ 
},, ˘

[C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\EventosMDFe\EventoCancelar.cs
	namespace'' 	
MDFe''
 
.'' 
Servicos'' 
.'' 
EventosMDFe'' #
{(( 
public)) 

class)) 
EventoCancelar)) 
{** 
public++ 
MDFeRetEventoMDFe++  
MDFeEventoCancelar++! 3
(++3 4
MDFeEletronico++4 B
mdfe++C G
,++G H
byte++I M
sequenciaEvento++N ]
,++] ^
string++_ e
	protocolo++f o
,++o p
string++q w
justificativa	++x Ö
)
++Ö Ü
{,, 	
var-- 
cancelamento-- 
=-- 
ClassesFactory-- -
.--- .
CriaEvCancMDFe--. <
(--< =
	protocolo--= F
,--F G
justificativa--H U
)--U V
;--V W
var// 
retorno// 
=// 
new// 
ServicoController// /
(/// 0
)//0 1
.//1 2
Executar//2 :
(//: ;
mdfe//; ?
,//? @
sequenciaEvento//A P
,//P Q
cancelamento//R ^
,//^ _
MDFeTipoEvento//` n
.//n o
Cancelamento//o {
)//{ |
;//| }
return11 
retorno11 
;11 
}22 	
}33 
}44 ∫

_C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\EventosMDFe\EventoEncerramento.cs
	namespace'' 	
MDFe''
 
.'' 
Servicos'' 
.'' 
EventosMDFe'' #
{(( 
public)) 

class)) 
EventoEncerramento)) #
{** 
public++ 
MDFeRetEventoMDFe++  "
MDFeEventoEncerramento++! 7
(++7 8
MDFeEletronico++8 F
mdfe++G K
,++K L
byte++M Q
sequenciaEvento++R a
,++a b
string++c i
	protocolo++j s
)++s t
{,, 	
var-- 
encerramento-- 
=-- 
ClassesFactory-- -
.--- .
CriaEvEncMDFe--. ;
(--; <
mdfe--< @
,--@ A
	protocolo--B K
)--K L
;--L M
var// 
retorno// 
=// 
new// 
ServicoController// /
(/// 0
)//0 1
.//1 2
Executar//2 :
(//: ;
mdfe//; ?
,//? @
sequenciaEvento//A P
,//P Q
encerramento//R ^
,//^ _
MDFeTipoEvento//` n
.//n o
Encerramento//o {
)//{ |
;//| }
return11 
retorno11 
;11 
}22 	
}33 
}44 Ö
cC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\EventosMDFe\EventoInclusaoCondutor.cs
	namespace'' 	
MDFe''
 
.'' 
Servicos'' 
.'' 
EventosMDFe'' #
{(( 
public)) 

class)) "
EventoInclusaoCondutor)) '
{** 
public++ 
MDFeRetEventoMDFe++  %
MDFeEventoIncluirCondutor++! :
(++: ;
MDFeEletronico++; I
mdfe++J N
,++N O
byte++P T
sequenciaEvento++U d
,++d e
string++f l
nome++m q
,++q r
string++s y
cpf++z }
)++} ~
{,, 	
var-- 
incluirCodutor-- 
=--  
ClassesFactory--! /
.--/ 0!
CriaEvIncCondutorMDFe--0 E
(--E F
nome--F J
,--J K
cpf--L O
)--O P
;--P Q
var// 
retorno// 
=// 
new// 
ServicoController// /
(/// 0
)//0 1
.//1 2
Executar//2 :
(//: ;
mdfe//; ?
,//? @
sequenciaEvento//A P
,//P Q
incluirCodutor//R `
,//` a
MDFeTipoEvento//b p
.//p q
InclusaoDeCondutor	//q É
)
//É Ñ
;
//Ñ Ö
return11 
retorno11 
;11 
}22 	
}33 
}44 ≠
ZC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\EventosMDFe\FactoryEvento.cs
	namespace** 	
MDFe**
 
.** 
Servicos** 
.** 
EventosMDFe** #
{++ 
public,, 

static,, 
class,, 
FactoryEvento,, %
{-- 
public.. 
static.. 
MDFeEventoMDFe.. $

CriaEvento..% /
(../ 0
MDFeEletronico..0 >
MDFe..? C
,..C D
MDFeTipoEvento..E S

tipoEvento..T ^
,..^ _
byte..` d
sequenciaEvento..e t
,..t u 
MDFeEventoContainer	..v â
evento
..ä ê
)
..ê ë
{// 	
var00 

eventoMDFe00 
=00 
new00  
MDFeEventoMDFe00! /
{11 
Versao22 
=22 
MDFeConfiguracao22 )
.22) *
VersaoWebService22* :
.22: ;$
VersaoMDFeRecepcaoEvento22; S
,22S T
	InfEvento33 
=33 
new33 
MDFeInfEvento33  -
{44 
Id55 
=55 
$str55 
+55 
(55  !
long55! %
)55% &

tipoEvento55& 0
+551 2
MDFe553 7
.557 8
Chave558 =
(55= >
)55> ?
+55@ A
sequenciaEvento55B Q
.55Q R
ToString55R Z
(55Z [
$str55[ _
)55_ `
,55` a
TpAmb66 
=66 
MDFeConfiguracao66 ,
.66, -
VersaoWebService66- =
.66= >
TipoAmbiente66> J
,66J K
CNPJ77 
=77 
MDFe77 
.77  
CNPJEmitente77  ,
(77, -
)77- .
,77. /
COrgao88 
=88 
MDFe88 !
.88! "

UFEmitente88" ,
(88, -
)88- .
,88. /
ChMDFe99 
=99 
MDFe99 !
.99! "
Chave99" '
(99' (
)99( )
,99) *
	DetEvento:: 
=:: 
new::  #
MDFeDetEvento::$ 1
{;; 
VersaoServico<< %
=<<& '
VersaoServico<<( 5
.<<5 6
	Versao100<<6 ?
,<<? @
EventoContainer== '
===( )
evento==* 0
}>> 
,>> 
DhEvento?? 
=?? 
DateTime?? '
.??' (
Now??( +
,??+ ,

NSeqEvento@@ 
=@@  
sequenciaEvento@@! 0
,@@0 1
TpEventoAA 
=AA 

tipoEventoAA )
}BB 
}CC 
;CC 

eventoMDFeEE 
.EE 
AssinarEE 
(EE 
)EE  
;EE  !
returnGG 

eventoMDFeGG 
;GG 
}HH 	
}II 
}JJ §
^C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\EventosMDFe\ServicoController.cs
	namespace)) 	
MDFe))
 
.)) 
Servicos)) 
.)) 
EventosMDFe)) #
{** 
public++ 

class++ 
ServicoController++ "
:++# $
IServicoController++% 7
{,, 
public-- 
MDFeRetEventoMDFe--  
Executar--! )
(--) *
MDFeEletronico--* 8
mdfe--9 =
,--= >
byte--? C
sequenciaEvento--D S
,--S T
MDFeEventoContainer--U h
eventoContainer--i x
,--x y
MDFeTipoEvento	--z à

tipoEvento
--â ì
)
--ì î
{.. 	
var// 
evento// 
=// 
FactoryEvento// &
.//& '

CriaEvento//' 1
(//1 2
mdfe//2 6
,//6 7

tipoEvento00 
,00 
sequenciaEvento11 
,11  
eventoContainer22 
)22  
;22  !
evento44 
.44 
ValidarSchema44  
(44  !
)44! "
;44" #
evento55 
.55 
SalvarXmlEmDisco55 #
(55# $
mdfe55$ (
.55( )
Chave55) .
(55. /
)55/ 0
)550 1
;551 2
var77 

webService77 
=77 
WsdlFactory77 (
.77( )&
CriaWsdlMDFeRecepcaoEvento77) C
(77C D
)77D E
;77E F
var88 

retornoXml88 
=88 

webService88 '
.88' (
mdfeRecepcaoEvento88( :
(88: ;
evento88; A
.88A B
CriaXmlRequestWs88B R
(88R S
)88S T
)88T U
;88U V
var:: 
retorno:: 
=:: 
MDFeRetEventoMDFe:: +
.::+ ,
LoadXml::, 3
(::3 4

retornoXml::4 >
.::> ?
OuterXml::? G
,::G H
evento::I O
)::O P
;::P Q
retorno;; 
.;; 
SalvarXmlEmDisco;; $
(;;$ %
mdfe;;% )
.;;) *
Chave;;* /
(;;/ 0
);;0 1
);;1 2
;;;2 3
return== 
retorno== 
;== 
}>> 	
}?? 
}@@ è
^C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\EventosMDFe\ServicoMDFeEvento.cs
	namespace$$ 	
MDFe$$
 
.$$ 
Servicos$$ 
.$$ 
EventosMDFe$$ #
{%% 
public&& 

class&& 
ServicoMDFeEvento&& "
{'' 
public(( 
MDFeRetEventoMDFe((  %
MDFeEventoIncluirCondutor((! :
(((: ;
MDFeEletronica)) 
mdfe)) 
,))  
byte))! %
sequenciaEvento))& 5
,))5 6
string))7 =
nome))> B
,))B C
string** 
cpf** 
)** 
{++ 	
var,, !
eventoIncluirCondutor,, %
=,,& '
new,,( +"
EventoInclusaoCondutor,,, B
(,,B C
),,C D
;,,D E
return.. !
eventoIncluirCondutor.. (
...( )%
MDFeEventoIncluirCondutor..) B
(..B C
mdfe..C G
,..G H
sequenciaEvento..I X
,..X Y
nome..Z ^
,..^ _
cpf..` c
)..c d
;..d e
}// 	
public11 
MDFeRetEventoMDFe11  8
,MDFeEventoEncerramentoMDFeEventoEncerramento11! M
(11M N
MDFeEletronica11N \
mdfe11] a
,11a b
byte11c g
sequenciaEvento11h w
,11w x
string11y 
	protocolo
11Ä â
)
11â ä
{22 	
var33 
eventoEncerramento33 "
=33# $
new33% (
EventoEncerramento33) ;
(33; <
)33< =
;33= >
return55 
eventoEncerramento55 %
.55% &"
MDFeEventoEncerramento55& <
(55< =
mdfe55= A
,55A B
sequenciaEvento55C R
,55R S
	protocolo55T ]
)55] ^
;55^ _
}66 	
public88 
MDFeRetEventoMDFe88  
MDFeEventoCancelar88! 3
(883 4
MDFeEletronica884 B
mdfe88C G
,88G H
byte88I M
sequenciaEvento88N ]
,88] ^
string88_ e
	protocolo88f o
,88o p
string99 
justificativa99  
)99  !
{:: 	
var;; 
eventoCancelamento;; "
=;;# $
new;;% (
EventoCancelar;;) 7
(;;7 8
);;8 9
;;;9 :
return== 
eventoCancelamento== %
.==% &
MDFeEventoCancelar==& 8
(==8 9
mdfe==9 =
,=== >
sequenciaEvento==? N
,==N O
	protocolo==P Y
,==Y Z
justificativa==[ h
)==h i
;==i j
}>> 	
}?? 
}@@ Â6
WC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\Factory\ClassesFactory.cs
	namespace-- 	
MDFe--
 
.-- 
Servicos-- 
.-- 
Factory-- 
{.. 
public// 

static// 
class// 
ClassesFactory// &
{00 
public11 
static11 
MDFeCosMDFeNaoEnc11 '
CriarConsMDFeNaoEnc11( ;
(11; <
string11< B
cnpj11C G
)11G H
{22 	
var33 
consMDFeNaoEnc33 
=33  
new33! $
MDFeCosMDFeNaoEnc33% 6
{44 
CNPJ55 
=55 
cnpj55 
,55 
TpAmb66 
=66 
MDFeConfiguracao66 (
.66( )
VersaoWebService66) 9
.669 :
TipoAmbiente66: F
,66F G
Versao77 
=77 
MDFeConfiguracao77 )
.77) *
VersaoWebService77* :
.77: ; 
VersaoMDFeConsNaoEnc77; O
,77O P
XServ88 
=88 
$str88 2
}99 
;99 
return;; 
consMDFeNaoEnc;; !
;;;! "
}<< 	
public>> 
static>> 
MDFeConsSitMDFe>> %
CriarConsSitMDFe>>& 6
(>>6 7
string>>7 =
chave>>> C
)>>C D
{?? 	
var@@ 
consSitMdfe@@ 
=@@ 
new@@ !
MDFeConsSitMDFe@@" 1
{AA 
VersaoBB 
=BB 
MDFeConfiguracaoBB )
.BB) *
VersaoWebServiceBB* :
.BB: ;
VersaoMDFeConsultaBB; M
,BBM N
TpAmbCC 
=CC 
MDFeConfiguracaoCC (
.CC( )
VersaoWebServiceCC) 9
.CC9 :
TipoAmbienteCC: F
,CCF G
XServDD 
=DD 
$strDD #
,DD# $
ChMDFeEE 
=EE 
chaveEE 
}FF 
;FF 
returnHH 
consSitMdfeHH 
;HH 
}II 	
publicKK 
staticKK 
MDFeEvCancMDFeKK $
CriaEvCancMDFeKK% 3
(KK3 4
stringKK4 :
	protocoloKK; D
,KKD E
stringKKF L
justificativaKKM Z
)KKZ [
{LL 	
varMM 
cancelamentoMM 
=MM 
newMM "
MDFeEvCancMDFeMM# 1
{NN 

DescEventoOO 
=OO 
$strOO +
,OO+ ,
NProtPP 
=PP 
	protocoloPP !
,PP! "
XJustQQ 
=QQ 
justificativaQQ %
}RR 
;RR 
returnTT 
cancelamentoTT 
;TT  
}UU 	
publicWW 
staticWW 
MDFeEvEncMDFeWW #
CriaEvEncMDFeWW$ 1
(WW1 2
MDFeEletronicoWW2 @
mdfeWWA E
,WWE F
stringWWG M
	protocoloWWN W
)WWW X
{XX 	
varYY 
encerramentoYY 
=YY 
newYY "
MDFeEvEncMDFeYY# 0
{ZZ 
CUF[[ 
=[[ 
mdfe[[ 
.[[ 

UFEmitente[[ %
([[% &
)[[& '
,[[' (
DtEnc\\ 
=\\ 
DateTime\\  
.\\  !
Now\\! $
,\\$ %

DescEvento]] 
=]] 
$str]] +
,]]+ ,
CMun^^ 
=^^ 
mdfe^^ 
.^^ '
CodigoIbgeMunicipioEmitente^^ 7
(^^7 8
)^^8 9
,^^9 :
NProt__ 
=__ 
	protocolo__ !
}`` 
;`` 
returnbb 
encerramentobb 
;bb  
}cc 	
publicee 
staticee !
MDFeEvIncCondutorMDFeee +!
CriaEvIncCondutorMDFeee, A
(eeA B
stringeeB H
nomeeeI M
,eeM N
stringeeO U
cpfeeV Y
)eeY Z
{ff 	
vargg 
condutorgg 
=gg 
newgg 
MDFeCondutorIncluirgg 2
{hh 
XNomeii 
=ii 
nomeii 
,ii 
CPFjj 
=jj 
cpfjj 
}kk 
;kk 
varmm 
incluirCodutormm 
=mm  
newmm! $!
MDFeEvIncCondutorMDFemm% :
{nn 

DescEventooo 
=oo 
$stroo 0
,oo0 1
Condutorpp 
=pp 
condutorpp #
}qq 
;qq 
returnss 
incluirCodutorss !
;ss! "
}tt 	
publicvv 
staticvv 
MDFeEnviMDFevv "
CriaEnviMDFevv# /
(vv/ 0
longvv0 4
lotevv5 9
,vv9 :
MDFeEletronicovv; I
mdfevvJ N
)vvN O
{ww 	
varxx 
enviMdfexx 
=xx 
newxx 
MDFeEnviMDFexx +
{yy 
MDFezz 
=zz 
mdfezz 
,zz 
IdLote{{ 
={{ 
lote{{ 
.{{ 
ToString{{ &
({{& '
){{' (
,{{( )
Versao|| 
=|| 
VersaoServico|| &
.||& '
	Versao100||' 0
}}} 
;}} 
return 
enviMdfe 
; 
}
ÄÄ 	
public
ÇÇ 
static
ÇÇ 
MDFeConsReciMDFe
ÇÇ &
CriaConsReciMDFe
ÇÇ' 7
(
ÇÇ7 8
string
ÇÇ8 >
numeroRecibo
ÇÇ? K
)
ÇÇK L
{
ÉÉ 	
var
ÑÑ 
consReciMDFe
ÑÑ 
=
ÑÑ 
new
ÑÑ "
MDFeConsReciMDFe
ÑÑ# 3
{
ÖÖ 
Versao
ÜÜ 
=
ÜÜ 
MDFeConfiguracao
ÜÜ )
.
ÜÜ) *
VersaoWebService
ÜÜ* :
.
ÜÜ: ;#
VersaoMDFeRetRecepcao
ÜÜ; P
,
ÜÜP Q
TpAmb
áá 
=
áá 
MDFeConfiguracao
áá (
.
áá( )
VersaoWebService
áá) 9
.
áá9 :
TipoAmbiente
áá: F
,
ááF G
NRec
àà 
=
àà 
numeroRecibo
àà #
}
ââ 
;
ââ 
return
ãã 
consReciMDFe
ãã 
;
ãã  
}
åå 	
public
éé 
static
éé "
MDFeConsStatServMDFe
éé *"
CriaConsStatServMDFe
éé+ ?
(
éé? @
)
éé@ A
{
èè 	
return
êê 
new
êê "
MDFeConsStatServMDFe
êê +
{
ëë 
TpAmb
íí 
=
íí 
MDFeConfiguracao
íí (
.
íí( )
VersaoWebService
íí) 9
.
íí9 :
TipoAmbiente
íí: F
,
ííF G
Versao
ìì 
=
ìì 
MDFeConfiguracao
ìì )
.
ìì) *
VersaoWebService
ìì* :
.
ìì: ;%
VersaoMDFeStatusServico
ìì; R
,
ììR S
XServ
îî 
=
îî 
$str
îî  
}
ïï 
;
ïï 
}
ññ 	
}
óó 
}òò Ê?
TC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\Factory\WsdlFactory.cs
	namespace,, 	
MDFe,,
 
.,, 
Servicos,, 
.,, 
Factory,, 
{-- 
public.. 

static.. 
class.. 
WsdlFactory.. #
{// 
public00 
static00 
MDFeConsNaoEnc00 $"
CriaWsdlMDFeConsNaoEnc00% ;
(00; <
)00< =
{11 	
var22 
url22 
=22 
	UrlHelper22 
.22  
ObterUrlServico22  /
(22/ 0
MDFeConfiguracao220 @
.22@ A
VersaoWebService22A Q
.22Q R
TipoAmbiente22R ^
)22^ _
.22_ `
MDFeConsNaoEnc22` n
;22n o
var33 
versao33 
=33 
MDFeConfiguracao33 )
.33) *
VersaoWebService33* :
.33: ; 
VersaoMDFeConsNaoEnc33; O
.33O P
GetVersaoString33P _
(33_ `
)33` a
;33a b
var44 
configuracaoWsdl44  
=44! "
CriaConfiguracao44# 3
(443 4
url444 7
,447 8
versao449 ?
)44? @
;44@ A
var66 
ws66 
=66 
new66 
MDFeConsNaoEnc66 '
(66' (
configuracaoWsdl66( 8
)668 9
;669 :
return77 
ws77 
;77 
}88 	
public:: 
static:: 
MDFeConsulta:: " 
CriaWsdlMDFeConsulta::# 7
(::7 8
)::8 9
{;; 	
var<< 
url<< 
=<< 
	UrlHelper<< 
.<<  
ObterUrlServico<<  /
(<</ 0
MDFeConfiguracao<<0 @
.<<@ A
VersaoWebService<<A Q
.<<Q R
TipoAmbiente<<R ^
)<<^ _
.<<_ `
MDFeConsulta<<` l
;<<l m
var== 
versao== 
=== 
MDFeConfiguracao== )
.==) *
VersaoWebService==* :
.==: ;
VersaoMDFeConsulta==; M
.==M N
GetVersaoString==N ]
(==] ^
)==^ _
;==_ `
var?? 
configuracaoWsdl??  
=??! "
CriaConfiguracao??# 3
(??3 4
url??4 7
,??7 8
versao??9 ?
)??? @
;??@ A
returnAA 
newAA 
MDFeConsultaAA #
(AA# $
configuracaoWsdlAA$ 4
)AA4 5
;AA5 6
}BB 	
publicDD 
staticDD 
MDFeRecepcaoEventoDD (&
CriaWsdlMDFeRecepcaoEventoDD) C
(DDC D
)DDD E
{EE 	
varFF 
urlFF 
=FF 
	UrlHelperFF 
.FF  
ObterUrlServicoFF  /
(FF/ 0
MDFeConfiguracaoFF0 @
.FF@ A
VersaoWebServiceFFA Q
.FFQ R
TipoAmbienteFFR ^
)FF^ _
.FF_ `
MDFeRecepcaoEventoFF` r
;FFr s
varGG 
versaoGG 
=GG 
MDFeConfiguracaoGG )
.GG) *
VersaoWebServiceGG* :
.GG: ;$
VersaoMDFeRecepcaoEventoGG; S
.GGS T
GetVersaoStringGGT c
(GGc d
)GGd e
;GGe f
varII 
configuracaoWsdlII  
=II! "
CriaConfiguracaoII# 3
(II3 4
urlII4 7
,II7 8
versaoII9 ?
)II? @
;II@ A
returnKK 
newKK 
MDFeRecepcaoEventoKK )
(KK) *
configuracaoWsdlKK* :
)KK: ;
;KK; <
}LL 	
publicNN 
staticNN 
MDFeRecepcaoNN " 
CriaWsdlMDFeRecepcaoNN# 7
(NN7 8
)NN8 9
{OO 	
varPP 
urlPP 
=PP 
	UrlHelperPP 
.PP  
ObterUrlServicoPP  /
(PP/ 0
MDFeConfiguracaoPP0 @
.PP@ A
VersaoWebServicePPA Q
.PPQ R
TipoAmbientePPR ^
)PP^ _
.PP_ `
MDFeRecepcaoPP` l
;PPl m
varQQ 
versaoServicoQQ 
=QQ 
MDFeConfiguracaoQQ  0
.QQ0 1
VersaoWebServiceQQ1 A
.QQA B
VersaoMDFeRecepcaoQQB T
.QQT U
GetVersaoStringQQU d
(QQd e
)QQe f
;QQf g
varSS 
configuracaoWsdlSS  
=SS! "
CriaConfiguracaoSS# 3
(SS3 4
urlSS4 7
,SS7 8
versaoServicoSS9 F
)SSF G
;SSG H
returnUU 
newUU 
MDFeRecepcaoUU #
(UU# $
configuracaoWsdlUU$ 4
)UU4 5
;UU5 6
}VV 	
publicXX 
staticXX 
MDFeRetRecepcaoXX %#
CriaWsdlMDFeRetRecepcaoXX& =
(XX= >
)XX> ?
{YY 	
varZZ 
urlZZ 
=ZZ 
	UrlHelperZZ 
.ZZ  
ObterUrlServicoZZ  /
(ZZ/ 0
MDFeConfiguracaoZZ0 @
.ZZ@ A
VersaoWebServiceZZA Q
.ZZQ R
TipoAmbienteZZR ^
)ZZ^ _
.ZZ_ `
MDFeRetRecepcaoZZ` o
;ZZo p
var[[ 
versao[[ 
=[[ 
MDFeConfiguracao[[ )
.[[) *
VersaoWebService[[* :
.[[: ;!
VersaoMDFeRetRecepcao[[; P
.[[P Q
GetVersaoString[[Q `
([[` a
)[[a b
;[[b c
var]] 
configuracaoWsdl]]  
=]]! "
CriaConfiguracao]]# 3
(]]3 4
url]]4 7
,]]7 8
versao]]9 ?
)]]? @
;]]@ A
return__ 
new__ 
MDFeRetRecepcao__ &
(__& '
configuracaoWsdl__' 7
)__7 8
;__8 9
}`` 	
publicbb 
staticbb 
MDFeStatusServicobb '%
CriaWsdlMDFeStatusServicobb( A
(bbA B
)bbB C
{cc 	
vardd 
urldd 
=dd 
	UrlHelperdd 
.dd  
ObterUrlServicodd  /
(dd/ 0
MDFeConfiguracaodd0 @
.dd@ A
VersaoWebServiceddA Q
.ddQ R
TipoAmbienteddR ^
)dd^ _
.dd_ `
MDFeStatusServicodd` q
;ddq r
varee 
versaoee 
=ee 
MDFeConfiguracaoee )
.ee) *
VersaoWebServiceee* :
.ee: ;#
VersaoMDFeStatusServicoee; R
.eeR S
GetVersaoStringeeS b
(eeb c
)eec d
;eed e
vargg 
configuracaoWsdlgg  
=gg! "
CriaConfiguracaogg# 3
(gg3 4
urlgg4 7
,gg7 8
versaogg9 ?
)gg? @
;gg@ A
returnii 
newii 
MDFeStatusServicoii (
(ii( )
configuracaoWsdlii) 9
)ii9 :
;ii: ;
}jj 	
privatell 
staticll 
WsdlConfiguracaoll '
CriaConfiguracaoll( 8
(ll8 9
stringll9 ?
urlll@ C
,llC D
stringllE K
versaollL R
)llR S
{mm 	
varnn 
codigoEstadonn 
=nn 
MDFeConfiguracaonn /
.nn/ 0
VersaoWebServicenn0 @
.nn@ A

UfEmitentennA K
.nnK L!
GetCodigoIbgeEmStringnnL a
(nna b
)nnb c
;nnc d
varoo 
certificadoDigitaloo "
=oo# $
MDFeConfiguracaooo% 5
.oo5 6
X509Certificate2oo6 F
;ooF G
returnqq 
newqq 
WsdlConfiguracaoqq '
{rr 
CertificadoDigitalss "
=ss# $
certificadoDigitalss% 7
,ss7 8
Versaott 
=tt 
versaott 
,tt  
CodigoIbgeEstadouu  
=uu! "
codigoEstadouu# /
,uu/ 0
Urlvv 
=vv 
urlvv 
,vv 
TimeOutww 
=ww 
MDFeConfiguracaoww *
.ww* +
VersaoWebServiceww+ ;
.ww; <
TimeOutww< C
}xx 
;xx 
}yy 	
}zz 
}{{ É
XC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str (
)( )
]) *
[ 
assembly 	
:	 

AssemblyDescription 
( 
$str !
)! "
]" #
[		 
assembly		 	
:			 
!
AssemblyConfiguration		  
(		  !
$str		! #
)		# $
]		$ %
[

 
assembly

 	
:

	 

AssemblyCompany

 
(

 
$str

 
)

 
]

 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str *
)* +
]+ ,
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
["" 
assembly"" 	
:""	 

AssemblyVersion"" 
("" 
$str"" $
)""$ %
]""% &
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## (
)##( )
]##) *ﬁ
aC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\RecepcaoMDFe\ServicoMDFeRecepcao.cs
	namespace'' 	
MDFe''
 
.'' 
Servicos'' 
.'' 
RecepcaoMDFe'' $
{(( 
public)) 

class)) 
ServicoMDFeRecepcao)) $
{** 
public++ 
MDFeRetEnviMDFe++ 
MDFeRecepcao++ +
(+++ ,
long++, 0
lote++1 5
,++5 6
MDFeEletronico++7 E
mdfe++F J
)++J K
{,, 	
var-- 
enviMDFe-- 
=-- 
ClassesFactory-- )
.--) *
CriaEnviMDFe--* 6
(--6 7
lote--7 ;
,--; <
mdfe--= A
)--A B
;--B C
enviMDFe// 
.// 
MDFe// 
.// 
Assina//  
(//  !
)//! "
;//" #
enviMDFe00 
.00 
Valida00 
(00 
)00 
;00 
enviMDFe11 
.11 
SalvarXmlEmDisco11 %
(11% &
)11& '
;11' (
var33 

webService33 
=33 
WsdlFactory33 (
.33( ) 
CriaWsdlMDFeRecepcao33) =
(33= >
)33> ?
;33? @
var44 

retornoXml44 
=44 

webService44 '
.44' (
mdfeRecepcaoLote44( 8
(448 9
enviMDFe449 A
.44A B
CriaXmlRequestWs44B R
(44R S
)44S T
)44T U
;44U V
var66 
retorno66 
=66 
MDFeRetEnviMDFe66 )
.66) *
LoadXml66* 1
(661 2

retornoXml662 <
.66< =
OuterXml66= E
,66E F
enviMDFe66G O
)66O P
;66P Q
retorno77 
.77 
SalvarXmlEmDisco77 $
(77$ %
)77% &
;77& '
return99 
retorno99 
;99 
}:: 	
};; 
}<< ø
gC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\RetRecepcaoMDFe\ServicoMDFeRetRecepcao.cs
	namespace&& 	
MDFe&&
 
.&& 
Servicos&& 
.&& 
RetRecepcaoMDFe&& '
{'' 
public(( 

class(( "
ServicoMDFeRetRecepcao(( '
{)) 
public** 
MDFeRetConsReciMDFe** "
MDFeRetRecepcao**# 2
(**2 3
string**3 9
numeroRecibo**: F
)**F G
{++ 	
var,, 
consReciMdfe,, 
=,, 
ClassesFactory,, -
.,,- .
CriaConsReciMDFe,,. >
(,,> ?
numeroRecibo,,? K
),,K L
;,,L M
consReciMdfe-- 
.-- 
ValidaSchema-- %
(--% &
)--& '
;--' (
consReciMdfe.. 
... 
SalvarXmlEmDisco.. )
(..) *
)..* +
;..+ ,
var00 

webService00 
=00 
WsdlFactory00 (
.00( )#
CriaWsdlMDFeRetRecepcao00) @
(00@ A
)00A B
;00B C
var11 

retornoXml11 
=11 

webService11 '
.11' (
mdfeRetRecepcao11( 7
(117 8
consReciMdfe118 D
.11D E
CriaRequestWs11E R
(11R S
)11S T
)11T U
;11U V
var33 
retorno33 
=33 
MDFeRetConsReciMDFe33 -
.33- .
LoadXml33. 5
(335 6

retornoXml336 @
.33@ A
OuterXml33A I
,33I J
consReciMdfe33K W
)33W X
;33X Y
retorno44 
.44 
SalvarXmlEmDisco44 $
(44$ %
)44% &
;44& '
return66 
retorno66 
;66 
}77 	
}88 
}99 °
kC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\MDFe\MDFe.Servicos\StatusServicoMDFe\ServicoMDFeStatusServico.cs
	namespace&& 	
MDFe&&
 
.&& 
Servicos&& 
.&& 
StatusServicoMDFe&& )
{'' 
public(( 

class(( $
ServicoMDFeStatusServico(( )
{)) 
public** 
MDFeRetConsStatServ** "
MDFeStatusServico**# 4
(**4 5
)**5 6
{++ 	
var,, 
consStatServMDFe,,  
=,,! "
ClassesFactory,,# 1
.,,1 2 
CriaConsStatServMDFe,,2 F
(,,F G
),,G H
;,,H I
consStatServMDFe-- 
.-- 
ValidarSchema-- *
(--* +
)--+ ,
;--, -
consStatServMDFe.. 
... 
SalvarXmlEmDisco.. -
(..- .
)... /
;../ 0
var00 

webService00 
=00 
WsdlFactory00 (
.00( )%
CriaWsdlMDFeStatusServico00) B
(00B C
)00C D
;00D E
var11 

retornoXml11 
=11 

webService11 '
.11' ( 
mdfeStatusServicoMDF11( <
(11< =
consStatServMDFe11= M
.11M N
CriaRequestWs11N [
(11[ \
)11\ ]
)11] ^
;11^ _
var33 
retorno33 
=33 
MDFeRetConsStatServ33 -
.33- .
LoadXml33. 5
(335 6

retornoXml336 @
.33@ A
OuterXml33A I
,33I J
consStatServMDFe33K [
)33[ \
;33\ ]
retorno44 
.44 
SalvarXmlEmDisco44 $
(44$ %
)44% &
;44& '
return66 
retorno66 
;66 
}88 	
}99 
}:: 