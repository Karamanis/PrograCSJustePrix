PrograCSJustePrix
=================

Projet C# BA3 Génie Elec Pass : Juste Prix 


Partie Serveur :
----------------

Le serveur possède une liste d'images correspond aux cadeaux
Dès la connexion d'un joueur, le serveur lui envoie 3 cadeaux aléatoirement (une voiture, un mobilier et un voyage)
Ensuite le serveur attend la réponse du joueur et lui renvoie si le prix proposé est supérieur,
inférieur ou égal à la somme des 3 cadeaux
Si le joueur trouve le juste prix, il remporte les cadeaux


Partie Client :
---------------

Le joueur se connecte au serveur
Il reçoit les images correspond aux différents cadeaux à gagner
Il propose un prix pour ces cadeaux
Il reçoit une réponse du serveur (Plus, Moins ou Egal)
Le joueur possède 60 secondes pour trouver le prix total des cadeaux
