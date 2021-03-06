
//
// Dynamsoft JavaScript Library for Basic Initiation of Dynamic Web TWAIN
// More info on DWT: http://www.dynamsoft.com/Products/WebTWAIN_Overview.aspx
//
// Copyright 2017, Dynamsoft Corporation 
// Author: Dynamsoft Team
// Version: 13.1
//
/// <reference path="dynamsoft.webtwain.initiate.js" />
var Dynamsoft = Dynamsoft || { WebTwainEnv: {} };

Dynamsoft.WebTwainEnv.AutoLoad = true;
///
Dynamsoft.WebTwainEnv.Containers = [{ContainerId:'dwtcontrolContainer', Width:270, Height:350}];

/// If you need to use multiple keys on the same server, you can combine keys and write like this 
/// Dynamsoft.WebTwainEnv.ProductKey = 'key1;key2;key3';
Dynamsoft.WebTwainEnv.ProductKey = 't0068UwAAAKL72B2xgXJCWOzrr0IWNqiAoTVWb/6Fo2Br+t2OKm7/0Sk+JC12bYcCCQ4Rh/EcgNFwENe3aGJECYYXaYfwuQc=;D9B59A2A1B3FD7A4A0C0A3B2909F3C6F0CB3DD68880F1F9E9F78460FE74A8A863D2E155D5C0C205F3B1DF23D7FA5EB4866508D8C777052124067A778FC9E3475E6A113244A3D9A70342DEC5125DAE5BC935E5BDD3EA350D4B7830C3D863A166DDF1E96DC6C27895211720F07';//'t0068UwAAAA/bBMR6s3bYaEHRq+SqFGxTCmAq0fsEe/mSY+9bxFfHMvATqgZDVBTbw5/yA/bZXSzZ3E7Acfq1lB/hNTeKPvg=';//'3D5699BEF07EA1CA63618ED9A55BB6D81B30136255FA440940689089C1732B85AA9EAAE09E6CC25EBB5672C5245F3267DD7E4C528E574735AC04B4A58236B119D89ABC41F9E669ED6D0F14306FEEA697FA2BE39316E2C03570186B7B0CAE854D8719553F478AC935AC7A85C41878BFFF698DDEA93F46F5A29E25AF1BBC148EABF2DBD077F7B9E57F61DD5A44488302A5C94DD45D276F68278513F722F0556C8797855A521109164EB1FC01AFA52563;t00936QAAAHn2j8J1CkV+t+LIpmqVcAWtR5+QSRgcl2+mIAct3PUUKT93bvI4tOLMYwJZJTARcUgJR5Z8DmHOIx5jncJf/5Bj+Df7IFPjz3Ji2jBKrzEZ1KIXM29nSS6X';

///
Dynamsoft.WebTwainEnv.Trial = true;
///
Dynamsoft.WebTwainEnv.ActiveXInstallWithCAB = false;


(function(){
    var p = document.location.protocol;
    if (p !== 'https:' && p !== 'http:')
		Dynamsoft.WebTwainEnv.ResourcesPath = 'https://www.dynamsoft.com/Demo/DWT/Resources';
	else
        Dynamsoft.WebTwainEnv.ResourcesPath = "../Scripts/Resources";
})();


/// All callbacks are defined in the dynamsoft.webtwain.install.js file, you can customize them.

// Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', function(){
// 		// webtwain has been inited
// });






