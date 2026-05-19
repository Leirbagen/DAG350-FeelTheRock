# DAG350-FeelTheRock


**Description:**
A rhythm-based rock game inspired by the *Guitar Hero* style. The main objective is to explore the evolution of rock music over time, categorized by decades. The game features levels dedicated to the most famous rock bands of each era, along with interesting trivia about them.

**Credits & Development Team:**

Programming Team: 


* **Gabriel Molina Molina** (Lead Developer), alongside: Andres Choque (Chart Helper), Jorge Quinquivi (Chart Helper, Developer).


Design Team: 


* **Lexi Aylin Legurguro Isnado** (Lead Art Direction) and **Pauline Silvia Avarado Chavez** (Co-Lead Art Direction), with various student contributors from the DAG350 class: Cristian Zenon Ávalos (Artist)... etc.


Director 


* **Jose Alex Ojeda Trigo** (Director).


A 2D interactive game developed in Unity, driven by robust C# programming.

Welcome to Feel The Rock! This is a collaborative game development project where I served as the Lead Developer. The main focus of this project was to build a structured, responsive, and engaging experience using the Unity engine.

<img width="1916" height="1077" alt="Screenshot 2026-05-18 120709" src="https://github.com/user-attachments/assets/54acc87d-1082-4a13-8af0-39283b185ed6" />


The primary goal of this project was to apply solid **Game Architecture** and **Object-Oriented Programming (OOP)** principles within a 2D environment. By leading the development of the core mechanics, this project demonstrates:


* **C# Scripting & OOP:** Writing clean, modular code utilizing an event-driven approach to efficiently decouple game states and UI updates.
* **Rhythm Mechanics & Audio Sync:** Implementing precise gameplay loops using the engine's internal audio clock for exact note spawning and dual-track synchronization, ensuring accuracy independent of fluctuating framerates.
* **Asynchronous Logic:** Extensive use of coroutines to handle non-blocking operations, including smooth UI animations, delayed scene transitions, and complex audio crossfading.
* **Data Persistence:** Managing local save states through JSON serialization, persistently handling player progression, high scores, and level unlocking across sessions.
* **Custom Editor Tooling:** Developing in-editor helper tools to record gameplay inputs in real-time and bake them directly into data containers, drastically streamlining the level design process.
* **Engine Mastery:** Utilizing a component-based architecture, prefabs, animation state machines, and canvas management for an optimized workflow.



Design Patterns Applied
* **Singleton & Persistent Systems:** Used for global systems and cross-scene controllers to handle background logic, data serialization, and seamless audio transitions without losing state between levels.
* **Observer Pattern:** Applied to decouple systems; entities broadcast state changes via events, allowing other elements (like UI or visual effects) to react independently without tight coupling.
* **Data-Driven Design:** Leveraged native data containers to store rhythm maps, timings, and track references, cleanly separating level data from the core spawner logic.
* **Separation of Concerns:** Divided game logic into highly specialized managers handling specific tasks (inputs, health, scoring), keeping the business logic completely isolated from the presentation and UI layers.



Code to unlock all levels: dag350feeltherock



<img width="692" height="388" alt="Screen Recording 2026-05-18 115902" src="https://github.com/user-attachments/assets/a3454004-d2da-46bd-95bb-a19fb9bb0279" />


SOME INTERFACES: 



<img width="692" height="388" alt="Screen Recording 2026-05-18 151417" src="https://github.com/user-attachments/assets/27f36673-4392-459a-a0f2-6193b9b5f3c9" />



GAMEPLAY: 



<img width="800" height="449" alt="ScreenRecording2026-05-18151306-ezgif com-cut" src="https://github.com/user-attachments/assets/60113a59-7f49-415a-b742-573d5a6ec4a3" />





LINK TO THE DEMO REEL: 
https://youtu.be/KEmJAQBCxuA?si=uiFbIaoPZKG-8RAR

LINK TO DOWNLOAD THE GAME: 
https://drive.google.com/drive/folders/1SqdIbKULUmlbE68mygWJ5qC_HVteDK0p







