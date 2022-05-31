<?php
include('./config.php');

//allow CORS
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, POST');
header("Access-Control-Allow-Headers: X-Requested-With");


$offset = 0;

//get offset is exist and not empty
if(isset($_GET["offset"]) && !empty($_GET["offset"])){
    $offset = htmlspecialchars($_GET["offset"]);
}


//requests to bdd to fetch 10 element based starting at the offset
// $req = $bdd->prepare("SELECT * FROM scores ORDER BY score ASC LIMIT 10 OFFSET ".$offset."");
$req = $bdd->prepare("SELECT * FROM scores LIMIT 10 OFFSET ".$offset."");
$req->execute(array());

$info = $req->fetchAll();

header('Content-Type: application/json'); 
echo json_encode($info);
?>