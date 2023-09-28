­

RC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Base\ConfiguracaoDanfe.cs
	namespace%% 	
NFe%%
 
.%% 
Danfe%% 
.%% 
Base%% 
{&& 
public'' 

class'' 
ConfiguracaoDanfe'' "
{(( 
public,, 
byte,, 
[,, 
],, 
	Logomarca,, 
{,,  !
get,," %
;,,% &
set,,' *
;,,* +
},,, -
public11 
bool11 
DocumentoCancelado11 &
{11' (
get11) ,
;11, -
set11. 1
;111 2
}113 4
public77 
Image77 
	ObterLogo77 
(77 
)77  
{88 	
if99 
(99 
	Logomarca99 
==99 
null99 !
)99! "
return:: 
null:: 
;:: 
var;; 
ms;; 
=;; 
new;; 
MemoryStream;; %
(;;% &
	Logomarca;;& /
);;/ 0
;;;0 1
var<< 
image<< 
=<< 
Image<< 
.<< 

FromStream<< (
(<<( )
ms<<) +
)<<+ ,
;<<, -
return== 
image== 
;== 
}>> 	
}?? 
}@@ §
MC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Base\Fontes\Fonte.cs
	namespace%% 	
NFe%%
 
.%% 
Danfe%% 
.%% 
Base%% 
.%% 
Fontes%% 
{&& 
public'' 

static'' 
class'' 
Fonte'' 
{(( 
public-- 
static-- 

FontFamily--  
CarregarDeByteArray--! 4
(--4 5
byte--5 9
[--9 :
]--: ;
fonte--< A
,--A B
out--C F!
PrivateFontCollection--G \
colecaoDeFonte--] k
)--k l
{.. 	
var// 
handle// 
=// 
GCHandle// !
.//! "
Alloc//" '
(//' (
fonte//( -
,//- .
GCHandleType/// ;
.//; <
Pinned//< B
)//B C
;//C D
try00 
{11 
var22 
ptr22 
=22 
Marshal22 !
.22! "*
UnsafeAddrOfPinnedArrayElement22" @
(22@ A
fonte22A F
,22F G
$num22H I
)22I J
;22J K
colecaoDeFonte33 
=33  
new33! $!
PrivateFontCollection33% :
(33: ;
)33; <
;33< =
colecaoDeFonte44 
.44 
AddMemoryFont44 ,
(44, -
ptr44- 0
,440 1
fonte442 7
.447 8
Length448 >
)44> ?
;44? @
return55 
colecaoDeFonte55 %
.55% &
Families55& .
[55. /
$num55/ 0
]550 1
;551 2
}66 
finally77 
{88 
handle99 
.99 
Free99 
(99 
)99 
;99 
}:: 
};; 	
}<< 
}== ©
GC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Base\IDanfe.cs
	namespace"" 	
NFe""
 
."" 
Danfe"" 
."" 
Base"" 
{## 
public$$ 

	interface$$ 
IDanfe$$ 
{%% 
void++ 
Imprimir++ 
(++ 
bool++ 
exibirDialogo++ (
=++) *
true+++ /
,++/ 0
string++1 7

impressora++8 B
=++C D
$str++E G
)++G H
;++H I
void11 
ExportarPdf11 
(11 
string11 
arquivo11  '
)11' (
;11( )
}22 
}33 ‘
GC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Base\Enumns.cs
	namespace"" 	
NFe""
 
."" 
Danfe"" 
."" 
Base"" 
{## 
public$$ 

enum$$ "
NfceDetalheVendaNormal$$ &
{%% 
NaoImprimir&& 
=&& 
$num&& 
,&& 
UmaLinha'' 
='' 
$num'' 
,'' 

DuasLinhas(( 
=(( 
$num(( 
})) 
public++ 

enum++ '
NfceDetalheVendaContigencia++ +
{,, 
UmaLinha-- 
=-- 
$num-- 
,-- 

DuasLinhas.. 
=.. 
$num.. 
}// 
public11 

enum11 
NfceModoImpressao11 !
{22 
MultiplasPaginas44 
,44 
UnicaPagina77 
}88 
}99 å
[C:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Base\NFCe\ConfiguracaoDanfeNfce.cs
	namespace"" 	
NFe""
 
."" 
Danfe"" 
."" 
Base"" 
."" 
NFCe"" 
{## 
public$$ 

class$$ !
ConfiguracaoDanfeNfce$$ &
:$$' (
ConfiguracaoDanfe$$) :
{%% 
public&& !
ConfiguracaoDanfeNfce&& $
(&&$ %"
NfceDetalheVendaNormal&&% ;
detalheVendaNormal&&< N
,&&N O'
NfceDetalheVendaContigencia'' '#
detalheVendaContigencia''( ?
,''? @
byte''A E
[''E F
]''F G
	logomarca''H Q
=''R S
null''T X
,''X Y
bool(( 
imprimeDescontoItem(( $
=((% &
false((' ,
,((, -
float((. 3
margemEsquerda((4 B
=((C D
$num((E I
,((I J
float((K P
margemDireita((Q ^
=((_ `
$num((a e
,((e f
NfceModoImpressao)) 
modoImpressao)) +
=)), -
NfceModoImpressao)). ?
.))? @
MultiplasPaginas))@ P
,))P Q
bool))R V
documentoCancelado))W i
=))j k
false))l q
)))q r
{** 	
DocumentoCancelado++ 
=++  
documentoCancelado++! 3
;++3 4
DetalheVendaNormal,, 
=,,  
detalheVendaNormal,,! 3
;,,3 4#
DetalheVendaContigencia-- #
=--$ %#
detalheVendaContigencia--& =
;--= >
	Logomarca.. 
=.. 
	logomarca.. !
;..! "
ImprimeDescontoItem// 
=//  !
imprimeDescontoItem//" 5
;//5 6
MargemEsquerda00 
=00 
margemEsquerda00 +
;00+ ,
MargemDireita11 
=11 
margemDireita11 )
;11) *
ModoImpressao22 
=22 
modoImpressao22 )
;22) *
}33 	
private88 !
ConfiguracaoDanfeNfce88 %
(88% &
)88& '
{99 	
DocumentoCancelado:: 
=::  
false::! &
;::& '
};; 	
public@@ "
NfceDetalheVendaNormal@@ %
DetalheVendaNormal@@& 8
{@@9 :
get@@; >
;@@> ?
set@@@ C
;@@C D
}@@E F
publicFF '
NfceDetalheVendaContigenciaFF *#
DetalheVendaContigenciaFF+ B
{FFC D
getFFE H
;FFH I
setFFJ M
;FFM N
}FFO P
publicKK 
boolKK 
ImprimeDescontoItemKK '
{KK( )
getKK* -
;KK- .
setKK/ 2
;KK2 3
}KK4 5
publicPP 
floatPP 
MargemEsquerdaPP #
{PP$ %
getPP& )
;PP) *
setPP+ .
;PP. /
}PP0 1
publicUU 
floatUU 
MargemDireitaUU "
{UU# $
getUU% (
;UU( )
setUU* -
;UU- .
}UU/ 0
public[[ 
NfceModoImpressao[[  
ModoImpressao[[! .
{[[/ 0
get[[1 4
;[[4 5
set[[6 9
;[[9 :
}[[; <
}\\ 
}]] Ê
YC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Base\NFe\ConfiguracaoDanfeNfe.cs
	namespace"" 	
NFe""
 
."" 
Danfe"" 
."" 
Base"" 
."" 
NFe"" 
{## 
public$$ 

class$$  
ConfiguracaoDanfeNfe$$ %
:$$& '
ConfiguracaoDanfe$$( 9
{%% 
public&& 
bool&& 

DuasLinhas&& 
{&&  
get&&! $
;&&$ %
set&&& )
;&&) *
}&&+ ,
public((  
ConfiguracaoDanfeNfe(( #
(((# $
byte(($ (
[((( )
](() *
	logomarca((+ 4
=((5 6
null((7 ;
,((; <
bool((= A

duasLinhas((B L
=((M N
true((O S
,((S T
bool((U Y
documentoCancelado((Z l
=((m n
false((o t
)((t u
{)) 	
	Logomarca** 
=** 
	logomarca** !
;**! "

DuasLinhas++ 
=++ 

duasLinhas++ #
;++# $
DocumentoCancelado,, 
=,,  
documentoCancelado,,! 3
;,,3 4
}-- 	
private22  
ConfiguracaoDanfeNfe22 $
(22$ %
)22% &
{33 	
DocumentoCancelado44 
=44  
false44! &
;44& '

DuasLinhas55 
=55 
true55 
;55 
}66 	
}77 
}88 ƒ
XC:\Users\SWDEV\Source\Repos\SW10.SWMANAGER\NFe\NFe.Danfe.Base\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str )
)) *
]* +
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
$str +
)+ ,
], -
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
$str"" &
)""& '
]""' (
[## 
assembly## 	
:##	 

AssemblyFileVersion## 
(## 
$str## *
)##* +
]##+ ,