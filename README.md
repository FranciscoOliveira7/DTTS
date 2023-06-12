# 🐦 Don't Touch the Spikes | Remake

Don't Touch the Spikes Remake trata-se de um remake de um pequeno jogo popular da Ketchapp que inclui a movimentação no estilo de flappy bird e alguns powerups com que o jogador interage

## Trabalho realizado por:

- [Francisco Oliveira](https://github.com/Sincopse) - 25979
- [João Sousa]([https://github.com/JPMini84) - 25613

# ⚙️ Funcionalidades

- O jogador tem como objetivo sobreviver ao espinhos que vão aparecendo no lado da tela, o jogador tem de se movimentar de forma a tocar o maior número de vezes nas paredes laterais sem tocar nos espinhos, nem nas bordas verticais.
- O personagem é controlado com o teclado
  - Espaço é usado para o personagem saltar no jogo
  - O esc no teclado fecha o jogo.
- Possui efeito sonoros.
- ##### PowerUps
  - Invencibilidade
    - Quando o jogador apanha este powerup ele fica invencível durante 5 segundos , tornando assim impossível morrer nessa duração.
  - Slow Motion
    - Quando o jogador apanha este powerup ele pode ativar o mesmo usando a tecla "E", após a ativação o movimento do jogador e dos espinhos é desacelarada por 5 segundos.
- ##### PowerDown
  - Crescimento Rápido
    - Quando o jogador apanha este powerdown ele aumenta o seu tamanho tornando-se mais difícil desviar-se dos espinhos.

- ##### Modos de Jogo
  - Clássico
    - No modo clássico a posição e a quantia de espinhos é aleatória e muda conforme o jogador toca na parede.
  - Espinho único
    - No modo espinho único existe apenas um grupo de espinhos em cada parede mas os mesmos movem se para cima e para baixo.

---

## 📁 Organização das Pastas e Ficheiros

📂DTTS<br>
 └📂GameObjects<br>
      └📂Collectables<br>
           └💾Collectable.cs<br>
           └💾Invincibility.cs<br>
           └💾SlowMotion.cs<br>
           └💾Thicc.cs<br>
      └💾GameObject.cs<br>
      └💾Component.cs<br>
      └💾Player.cs<br>
      └💾Spike.cs<br>
      └💾Wall.cs<br>
 └📂Scenes<br>
      └💾Scene.cs<br>
      └💾Level1.cs<br>
 └📁Content<br>
 └📁Controls<br>
      └💾Button.cs<br>

### . git

- Esta pasta permanece escondida a não ser que o utilizador revele hidden items, através de View > Show > Hidden Items.
- Contêm todos os ficheiros necessários para manipular o repositório do git.
