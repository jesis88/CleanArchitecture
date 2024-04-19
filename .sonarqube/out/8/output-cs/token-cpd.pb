›B
#E:\CleanArchitecture\API\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
( 
) 
. 
AddJsonOptions 
( 
options 
=> 
options &
.& '!
JsonSerializerOptions' <
.< =

Converters= G
.G H
AddH K
(K L
newL O#
JsonStringEnumConverterP g
(g h
)h i
)i j
)j k
;k l
builder 
. 
Services 
. 
AddApplication 
( 
builder 
. 
Configuration )
)) *
. 
AddInfrastructure 
( 
builder 
. 
Configuration ,
), -
;- .
builder 
. 
Host 
. 

UseSerilog 
( 
( 
context  
,  !
configuration" /
)/ 0
=>1 3
configuration4 A
.A B
ReadFromB J
.J K
ConfigurationK X
(X Y
contextY `
.` a
Configurationa n
)n o
)o p
;p q
var 
jwtKey 

= 
builder 
. 
Configuration "
[" #
$str# ,
], -
;- .
if 
( 
jwtKey 	
!=
 
null 
) 
{ 
builder 
. 
Services 
. 
AddAuthentication &
(& '
options' .
=>/ 1
{   
options!! 
.!! %
DefaultAuthenticateScheme!! )
=!!* +
JwtBearerDefaults!!, =
.!!= > 
AuthenticationScheme!!> R
;!!R S
options"" 
."" "
DefaultChallengeScheme"" &
=""' (
JwtBearerDefaults"") :
."": ; 
AuthenticationScheme""; O
;""O P
}## 
)## 
.$$ 
AddJwtBearer$$ 
($$ 
$str$$ 
,$$ 
options$$ #
=>$$$ &
{%% 
options&& 
.&& %
TokenValidationParameters&& )
=&&* +
new&&, /%
TokenValidationParameters&&0 I
{'' 	
ValidateIssuer(( 
=(( 
true(( !
,((! "
ValidateAudience)) 
=)) 
true)) #
,))# $
ValidateLifetime** 
=** 
true** #
,**# $$
ValidateIssuerSigningKey++ $
=++% &
true++' +
,+++ ,
ValidIssuer,, 
=,, 
builder,, !
.,,! "
Configuration,," /
[,,/ 0
$str,,0 <
],,< =
,,,= >
ValidAudience-- 
=-- 
builder-- #
.--# $
Configuration--$ 1
[--1 2
$str--2 >
]--> ?
,--? @
IssuerSigningKey.. 
=.. 
new.. " 
SymmetricSecurityKey..# 7
(..7 8
Encoding..8 @
...@ A
UTF8..A E
...E F
GetBytes..F N
(..N O
jwtKey..O U
)..U V
)..V W
,..W X
	ClockSkew// 
=// 
TimeSpan//  
.//  !
Zero//! %
}00 	
;00	 

}11 
)11 
;11 
}22 
builder55 
.55 
Services55 
.55 #
AddEndpointsApiExplorer55 (
(55( )
)55) *
;55* +
builder66 
.66 
Services66 
.66 
AddSwaggerGen66 
(66 
c66  
=>66! #
{77 
c88 
.88 

SwaggerDoc88 
(88 
$str88 
,88 
new88 
OpenApiInfo88 &
{88' (
Title88) .
=88/ 0
$str881 9
,889 :
Version88; B
=88C D
$str88E I
}88J K
)88K L
;88L M
c;; 
.;; !
AddSecurityDefinition;; 
(;; 
$str;; (
,;;( )
new;;* -!
OpenApiSecurityScheme;;. C
{<< 
Type== 
=== 
SecuritySchemeType== !
.==! "
Http==" &
,==& '
Scheme>> 
=>> 
$str>> 
,>> 
BearerFormat?? 
=?? 
$str?? 
,?? 
Description@@ 
=@@ 
$str@@ G
,@@G H
}AA 
)AA 
;AA 
cCC 
.CC "
AddSecurityRequirementCC 
(CC 
newCC  &
OpenApiSecurityRequirementCC! ;
{DD 
{EE 
newFF !
OpenApiSecuritySchemeFF -
{GG 
	ReferenceHH !
=HH" #
newHH$ '
OpenApiReferenceHH( 8
{II 
TypeJJ  
=JJ! "
ReferenceTypeJJ# 0
.JJ0 1
SecuritySchemeJJ1 ?
,JJ? @
IdKK 
=KK  
$strKK! -
}LL 
}MM 
,MM 
ArrayNN 
.NN 
EmptyNN 
<NN  
stringNN  &
>NN& '
(NN' (
)NN( )
}OO 
}PP 
)PP 
;PP 
}QQ 
)QQ 
;QQ 
varSS 
appSS 
=SS 	
builderSS
 
.SS 
BuildSS 
(SS 
)SS 
;SS 
ifVV 
(VV 
appVV 
.VV 
EnvironmentVV 
.VV 
IsDevelopmentVV !
(VV! "
)VV" #
)VV# $
{WW 
appXX 
.XX 

UseSwaggerXX 
(XX 
)XX 
;XX 
appYY 
.YY 
UseSwaggerUIYY 
(YY 
cYY 
=>YY 
{ZZ 
c[[ 	
.[[	 

SwaggerEndpoint[[
 
([[ 
$str[[ 4
,[[4 5
$str[[6 A
)[[A B
;[[B C
}\\ 
)\\ 
;\\ 
}]] 
app__ 
.__ $
UseSerilogRequestLogging__ 
(__ 
)__ 
;__ 
appaa 
.aa 
UseAuthenticationaa 
(aa 
)aa 
;aa 
appbb 
.bb 
UseAuthorizationbb 
(bb 
)bb 
;bb 
appdd 
.dd 
MapControllersdd 
(dd 
)dd 
;dd 
appee 
.ee 
UseHttpsRedirectionee 
(ee 
)ee 
;ee 
usinggg 
(gg 
vargg 

scopegg 
=gg 
appgg 
.gg 
Servicesgg 
.gg  
CreateScopegg  +
(gg+ ,
)gg, -
)gg- .
{hh 
varii 
roleManagerii 
=ii 
scopeii 
.ii 
ServiceProviderii +
.ii+ ,
GetRequiredServiceii, >
<ii> ?
RoleManagerii? J
<iiJ K
IdentityRoleiiK W
<iiW X
stringiiX ^
>ii^ _
>ii_ `
>ii` a
(iia b
)iib c
;iic d
varjj 
rolesjj 
=jj 
newjj 
[jj 
]jj 
{jj 
$strjj 
,jj  
$strjj! +
}jj, -
;jj- .
foreachkk 
(kk 
varkk 
rolekk 
inkk 
roleskk 
)kk 
{ll 
ifmm 

(mm 
!mm 
awaitmm 
roleManagermm 
.mm 
RoleExistsAsyncmm .
(mm. /
rolemm/ 3
)mm3 4
)mm4 5
awaitnn 
roleManagernn 
.nn 
CreateAsyncnn )
(nn) *
newnn* -
IdentityRolenn. :
(nn: ;
rolenn; ?
)nn? @
)nn@ A
;nnA B
}oo 
}pp 
apprr 
.rr 
Runrr 
(rr 
)rr 	
;rr	 
ü-
6E:\CleanArchitecture\API\Controllers\UserController.cs
	namespace 	
API
 
. 
Controllers 
{ 
[

 
Route

 

(


 
$str

 
)

 
]

 
[ 
ApiController 
] 
public 

class 
UserController 
(  
	IMediator  )
mediator* 2
)2 3
:4 5
ControllerBase6 D
{ 
private 
readonly 
	IMediator "
	_mediator# ,
=- .
mediator/ 7
;7 8
[ 	
AllowAnonymous	 
] 
[ 	
HttpPost	 
( 
$str  
)  !
]! "
public 
async 
Task 
< 
IActionResult '
>' (+
GetUserRegistrationDetailsAsync) H
(H I
[I J
FromBodyJ R
]R S
CreateUserCommandT e
commandf m
,m n
CancellationToken	o Ä
cancellationToken
Å í
)
í ì
{ 	
try 
{ 
var 
response 
= 
await $
	_mediator% .
.. /
Send/ 3
(3 4
command4 ;
,; <
cancellationToken= N
)N O
;O P
return 
Ok 
( 
response "
)" #
;# $
} 
catch 
( 
	Exception 
ex 
)  
{ 
return 

StatusCode !
(! "
$num" %
,% &
$"' )
$str) @
{@ A
exA C
.C D
MessageD K
}K L
"L M
)M N
;N O
} 
} 	
[ 	
AllowAnonymous	 
] 
[   	
HttpGet  	 
(   
$str   
)   
]   
public!! 
async!! 
Task!! 
<!! 
IActionResult!! '
>!!' ($
GetUserLoginDetailsAsync!!) A
(!!A B
[!!B C
	FromQuery!!C L
]!!L M
LoginCommand!!N Z
command!![ b
,!!b c
CancellationToken!!d u
cancellationToken	!!v á
)
!!á à
{"" 	
try## 
{$$ 
var%% 
response%% 
=%% 
await%% $
	_mediator%%% .
.%%. /
Send%%/ 3
(%%3 4
command%%4 ;
,%%; <
cancellationToken%%= N
)%%N O
;%%O P
return&& 
Ok&& 
(&& 
response&& "
)&&" #
;&&# $
}'' 
catch(( 
((( 
	Exception(( 
ex(( 
)((  
{)) 
return** 

StatusCode** !
(**! "
$num**" %
,**% &
$"**' )
$str**) @
{**@ A
ex**A C
.**C D
Message**D K
}**K L
"**L M
)**M N
;**N O
}++ 
},, 	
[.. 	
HttpPost..	 
(.. 
$str..  
)..  !
]..! "
public// 
async// 
Task// 
<// 
IActionResult// '
>//' (
RefreshToken//) 5
(//5 6
[//6 7
	FromQuery//7 @
]//@ A
RefreshTokenQuery//B S
query//T Y
,//Y Z
CancellationToken//[ l
cancellationToken//m ~
)//~ 
{00 	
try11 
{22 
var33 
response33 
=33 
await33 $
	_mediator33% .
.33. /
Send33/ 3
(333 4
query334 9
,339 :
cancellationToken33; L
)33L M
;33M N
return44 
Ok44 
(44 
response44 "
)44" #
;44# $
}55 
catch66 
(66 
	Exception66 
ex66 
)66  
{77 
return88 

StatusCode88 !
(88! "
$num88" %
,88% &
$"88' )
$str88) @
{88@ A
ex88A C
.88C D
Message88D K
}88K L
"88L M
)88M N
;88N O
}99 
}:: 	
[== 	
HttpGet==	 
(== 
$str== 
)== 
]== 
public>> 
async>> 
Task>> 
<>> 
IActionResult>> '
>>>' (
GetUserListAsync>>) 9
(>>9 :
[>>: ;
	FromQuery>>; D
]>>D E
GetUserListQuery>>E U
query>>V [
,>>[ \
CancellationToken>>] n
cancellationToken	>>o Ä
)
>>Ä Å
{?? 	
try@@ 
{AA 
varBB 
responseBB 
=BB 
awaitBB $
	_mediatorBB% .
.BB. /
SendBB/ 3
(BB3 4
queryBB4 9
,BB9 :
cancellationTokenBB; L
)BBL M
;BBM N
returnCC 
OkCC 
(CC 
responseCC "
)CC" #
;CC# $
}DD 
catchEE 
(EE 
	ExceptionEE 
exEE 
)EE  
{FF 
returnGG 

StatusCodeGG !
(GG! "
$numGG" %
,GG% &
$"GG' )
$strGG) @
{GG@ A
exGGA C
.GGC D
MessageGGD K
}GGK L
"GGL M
)GGM N
;GGN O
}HH 
}II 	
}KK 
}LL À
:E:\CleanArchitecture\API\Controllers\CustomerController.cs
	namespace 	
API
 
. 
Controllers 
{ 
[		 
Route		 

(		
 
$str		 
)		 
]		 
[

 
ApiController

 
]

 
public 

class 
CustomerController #
(# $
	IMediator$ -
mediator. 6
)6 7
:8 9
ControllerBase: H
{ 
private 
readonly 
	IMediator "
	_mediator# ,
=- .
mediator/ 7
;7 8
[ 	
	Authorize	 
( 
Roles 
= 
$str "
)" #
]# $
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
IActionResult 
TestAuthorization .
(. /
)/ 0
{ 	
return 
Ok 
( 
$str 2
)2 3
;3 4
} 	
[ 	
HttpPost	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
CreateCustomer) 7
(7 8!
CreateCustomerCommand8 M
commandN U
,U V
CancellationTokenW h
cancellationTokeni z
)z {
{ 	
try 
{ 
var 
response 
= 
await $
	_mediator% .
.. /
Send/ 3
(3 4
command4 ;
,; <
cancellationToken= N
)N O
;O P
return 
response 
!=  "
$num# $
?% &
throw' ,
new- 0%
InvalidOperationException1 J
(J K
$strK ^
)^ _
:` a
Okb d
(d e
responsee m
)m n
;n o
} 
catch 
( 
	Exception 
ex 
)  
{ 
return   

StatusCode   !
(  ! "
$num  " %
,  % &
$"  ' )
$str  ) @
{  @ A
ex  A C
.  C D
Message  D K
}  K L
"  L M
)  M N
;  N O
}!! 
}"" 	
}$$ 
}%% 