$(document).ready(function(){
  $("#collapse1").on("hide.bs.collapse", function(){
    $(".btn-tog").html('<span class="glyphicon glyphicon-chevron-right"></span> Chat');
  });
  $("#collapse1").on("show.bs.collapse", function(){
    $(".btn-tog").html('<span class="glyphicon glyphicon-chevron-down"></span> Chat');
  });
});