<?php

  header("Content-Type: image/jpeg"); // it will return image
  readfile("1px.png");


  include("./include.php");
  $connec = mysqli_connect(DB_SERVER, DB_USER, DB_PASS, DB_NAME);

  $ip = $_SERVER['REMOTE_ADDR'];
  $json  = file_get_contents("http://ipinfo.io/".$ip."/geo");
  $json  =  json_decode($json ,true);
  $country =  $json['country'];
  $region= $json['region'];
  $city = $json['city'];

  $currentDate = new DateTime();

  $stmt = $connec->prepare('UPDATE pixel SET ip = ?, pais = ?, region = ?, ciudad = ?, fechaleido = ?  WHERE id = ?');
  $stmt->bind_param('sssssi', $ip, $country, $region, $city, $currentDate->format('d-m-Y H:i:s'), $_GET['t']);
  $stmt->execute();
  $stmt->fetch();
  $stmt->close();




?>
