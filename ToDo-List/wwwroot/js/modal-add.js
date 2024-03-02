
import { resizeTextarea, getPriorityText, showBGColors, closeModalWindow, rootAddress } from './common.js';
import { allCards, showAllCards } from './site.js';

const handleShowAddNewCardModal = () => {
    const cardModal = document.querySelector(".card-modal");
    cardModal.classList.add("modal-show");

    cardModal.innerHTML = `
	<div class="modal-card-form-container">
        <div class="new-card-form_background">
            <form id="new-card-form-id" class="new-card-form">
               <textarea class="input-title" placeholder="Enter card name"></textarea>
               <textarea class="input-text" placeholder="Enter text"></textarea>
            </form>
            <div class="new-card-form-navigation">
                <div class="priority-slider-container">
                    <div class="priority-label">Priority: Low</div>
                    <input type="range" min="1" max="3" value="1" class="priority-slider">
                </div>
                <button type="button" class="new-card-form_color-button"><i class="fa-solid fa-palette"></i></button>
                <button type="button" class="new-card-form_btn new-card-form_btn-confirm">Confirm</button>
                <button class="new-card-form_btn new-card-form_btn-close">Cancel</button>
            </div>
        </div>
        <div class="bg-color-container">
            <div class="color-option" data-color="white"></div>
            <div class="color-option" data-color="#ffb5a7"></div>
            <div class="color-option" data-color="#d0f4de"></div>
            <div class="color-option" data-color="#bde0fe"></div>
        </div>
    </div>
  `;
    //Card title
    const titleEl = document.querySelector('.input-title');
    titleEl.addEventListener('input', (element) => resizeTextarea(element));

    //Card text
    const bodytextEl = document.querySelector('.input-text');
    bodytextEl.addEventListener('input', (element) => resizeTextarea(element));

    //Priority slider
    const sliderEl = document.querySelector('.priority-slider');
    const labelEl = document.querySelector('.priority-label');
    sliderEl.addEventListener('input', function () {
        let priority = parseInt(sliderEl.value);

        labelEl.textContent = 'Priority: ' + getPriorityText(priority);
    });

    //Color button (shows colors options)
    const colorBtnEl = document.querySelector('.new-card-form_color-button');
    colorBtnEl.addEventListener('click', (element) => showBGColors(element));

    //Confirm button (sending new card to the server)
    const confirmBtn = document.querySelector('.new-card-form_btn-confirm');
    confirmBtn.addEventListener('click', () => sendNewCard());

    //Cancel button (closing the modal window)
    const cancelBtn = document.querySelector('.new-card-form_btn-close');
    cancelBtn.addEventListener('click', () => closeModalWindow());
};

function sendNewCard() {
    let newTaskCard = {
        title: document.querySelector(".input-title").value,
        text: document.querySelector(".input-text").value,
        backgroundColor: window.getComputedStyle(document.querySelector(".new-card-form_background")).backgroundColor,
        priority: parseInt(document.querySelector(".priority-slider").value)
    };

    let requestOptions = {
        method: 'POST',
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify(newTaskCard)
    };


    fetch(`${rootAddress}/api/Task/add-card`, requestOptions)
        .then(response => {
            if (response.status === 400) {
                throw new Error('Something went wrong');
            }
            return response.json();
        })
        .then(response => {
            response.editedDate = new Date(response.editedDate);

            allCards.push(response);
            showAllCards(allCards);

            alert("Congrats! Now you have a new task");
            closeModalWindow();
        })
        .catch(error => {
            alert(`${error}. Try to add task again`);
        });
}


export { handleShowAddNewCardModal };