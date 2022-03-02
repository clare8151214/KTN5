

function ShowTime(){
    　var NowDate=new Date();
      var Y=NowDate.getUTCFullYear();
      var M=NowDate.getMonth();
      var D=NowDate.getDate();
    　var h=NowDate.getHours();
    　var m=NowDate.getMinutes();
    　document.getElementById('showbox').innerHTML = Y+'年'+(M+1)+'月'+D+'日'+h+'時'+m+'分';
    　setTimeout('ShowTime()');
    }