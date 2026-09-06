# GunMecha & Run- Run & Gun 2D TP02 - ST2OOS

Mini-jeu 2D de type Run & Gun inspiré de Metal Slug, réalisé sous Unity.



## Fonctionnalités réalisées

### Personnage principal
- Déplacement horizontal (Q/D) et saut (Z/Espace) au clavier
- Le personnage regarde la direction où il avance
- Animations Idle, Run, Jump gérées via un Animator Controller avec transitions basées sur des paramètres (Speed, IsGrounded, VerticalVelocity)

### Système de tir multidirectionnel
- Tir dans 8 directions (gauche, droite, haut, bas en l'air, et diagonales) avec les touches I/J/K/L
- Orientation visuelle du projectile cohérente avec sa trajectoire
- Animations de tir différenciées selon l'état du personnage (immobile, en course, en saut) et la direction visée, il n'y a pas l'angle d'animation pour les diagonales

### Ennemis
- **Ennemi de mêlée** : détection du joueur via Collider2D en Trigger, approche automatique, attaque au contact avec un temps d'attente
- **Ennemi à distance** : détection du joueur, ne bouge pas, tir de projectiles dans sa direction, animation de tir dédiée

### événement
- Zones de détection et d'attaque des ennemis en Collider2D Trigger
- Zone de fin de niveau redirigeant vers le niveau suivant ou le menu principal


### Dégâts et vie
- Système de points de vie entiers côté joueur et ennemis
- Le joueur subit des dégâts des attaques de mêlée et des projectiles ennemis
- Invincibilité temporaire avec un clignotement visuel après un coup reçu
- Barre de vie (Slider UI) mise à jour en temps réel
- Animation de mort + écran Game Over avec bouton Rejouer

### Level design
- Deux niveaux distincts (Level 1 et Level 2) avec différents décors (plateformes, obstacles, ennemis)
- Plateformes traversables par le dessous (Platform Effector 2D)
- Caméra qui suit le joueur, background qui suit aussi mais avec un petit retard
- Menu principal permettant de choisir le niveau à jouer

### Organisation du projet
- Scripts C# séparés par responsabilité (PlayerController, PlayerHealth, EnemyHealth, MeleeEnemy, RangedEnemy, Projectile, EnemyProjectile, CameraFollow, BackgroundFollow, LevelFinish, CombatZoneTrigger, Menu)
- Prefabs pour les éléments réutilisables (projectiles)
- Scènes séparées par niveau

