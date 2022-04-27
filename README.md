# Xenor

## Notes importantes

**Nous utilisons github comme plateforme d'échange principal. Les codes sont syncronisé sur le cri par l'intermédiaire d'un bot.**
[Lien du github](https://github.com/s2xenor/xenor/)

## Sommaire
- [Présentation du projet](#project_presentation)
- [Organisation des répertoires](#file_organization)

## Présentation du projet<a id="project_presentation"></a>

Vous vous réveillez dans une cellule, en face de vous, derrière un mur de verre se trouve un autre prisonnier. 
Clothilde, une ancienne étudiante de l’EPITA, vous aide à vous évader. 
Malheureusement, elle ne parvient à déverrouiller la porte principale du labo. 
Il vous faudra donc en trouver les clefs. 
Mais pour cela il faudra affronter les monstres crée par les scientifiques lors de leurs manipulations géniques et résoudre les énigmes qui permettent de passer d’une salle du labo à l’autre… 
Mais si Clothilde n’était qu’en réalité qu’une autre scientifique qui évalue vos capacités…

## Organisation des répertoires<a id="file_organization"></a>

* ``/`` : le jeux en lui même
    * ``/Assets/GeneralObjects`` : toutes le contenu du jeu
    *     ``/Assets/GeneralObjects/Canvas`` : le système d'affichage sur écran du joueur
        * ``/Assets/GeneralObjects/DialogSystem`` : le système de dialogue
        * ``/Assets/GeneralObjects/Doors`` : le système de portes
        * ``/Assets/GeneralObjects/Enigm`` : les différentes enigmes
        * ``/Assets/GeneralObjects/Monster`` : le système de monstre
        * ``/Assets/GeneralObjects/Potion`` : le système de potion
        * ``/Assets/GeneralObjects/Player`` : les joueurs
    * ``/Assets/PlaceHoldersSprites`` : les sprites pour les salles
    * ``/Assets/Resources`` : ressources du jeu qui sont intégrées au build

* ``/Narration`` : tout le scénario du jeu

* ``/launcher/AlteraVita`` : le launcher

* ``/site`` : le site web
    * ``/site/front`` : le front du site
    * ``/site/back`` : le backend du site


