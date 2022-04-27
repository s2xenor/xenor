# Xenor

## Notes importantes

**Nous utilisons github comme plateforme d'échange principal. Les codes sont syncronisé sur le cri par l'intermédiaire d'un bot.**

[Lien du github](https://github.com/s2xenor/xenor/)

[Lien du site web](https://s2xenor.github.io/xenor/site/front)


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

* ``/Assets``
    * ``/GeneralObjects`` : toutes le contenu du jeu
        * ``/Canvas`` : le système d'affichage sur écran du joueur
        * ``/DialogSystem`` : le système de dialogue
        * ``/Doors`` : le système de portes
        * ``/Enigm`` : les différentes enigmes
        * ``/Monster`` : le système de monstre
        * ``/Potion`` : le système de potion
        * ``/Player`` : les joueurs
    * ``/PlaceHoldersSprites`` : les sprites pour les salles
    * ``/Resources`` : ressources du jeu qui sont intégrées au build

* ``/Narration`` : tout le scénario du jeu

* ``/launcher/AlteraVita`` : le launcher

* ``/site`` : le site web
    * ``/front`` : le front du site
    * ``/back`` : le backend du site


