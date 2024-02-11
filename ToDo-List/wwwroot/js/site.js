'use strict';

const rootAddress = "https://localhost:7274";

const mainContentEl = document.querySelector(".main-content");
const checkboxInProgressEl = document.querySelector(".toggle-checkbox-in-progress");
const checkboxDoneEl = document.querySelector(".toggle-checkbox-done");

let allCards = JSON.parse(document.getElementById("allCards").value);
allCards.forEach(x => x.editedDate = new Date(x.editedDate));
showAllCards();

//Modal window section start
const cardModal = document.querySelector(".card-modal");

const handleShowAddNewCardModal = () => {
    cardModal.classList.add("modal-show");

    cardModal.innerHTML = `
	<div class="modal-card-form-container">
        <div class="new-card-form_background">
            <form id="new-card-form-id" class="new-card-form">
               <textarea class="input-title" placeholder="Enter card name" onInput="resizeTextarea(event)"/></textarea>
               <textarea class="input-text" placeholder="Enter text" onInput="resizeTextarea(event)"></textarea>
            </form>
            <div class="new-card-form-navigation">
                <div class="priority-slider-container">
                    <div class="priority-label">Priority: Low</div>
                    <input type="range" min="1" max="3" value="1" class="priority-slider">
                </div>
                <button onClick="showBGColors(event)" type="button" class="new-card-form_color-button"><i class="fa-solid fa-palette"></i></button>
                <button type="button" onClick="sendNewCard()" class="new-card-form_btn">Confirm</button>
                <button onClick="closeModalWindow()" class="new-card-form_btn new-card-form_btn-close">Cancel</button>
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
    
    const slider = document.querySelector('.priority-slider');
    const label = document.querySelector('.priority-label');

    slider.addEventListener('input', function () {
        let priority = parseInt(slider.value);

        label.textContent = 'Priority: ' + getPriorityText(priority);
    });
};

function resizeTextarea(event) {
    let currentEl = event.currentTarget;
    // Narrow
    currentEl.style.height = 'auto';
    // Expand
    currentEl.style.height = currentEl.scrollHeight + 'px';
}

function getPriorityText(priorityValue){
    switch (priorityValue) {
        case 1:
            return 'Low';
            break;
        case 2:
            return 'Medium';
            break;
        case 3:
            return 'High';
            break;
        default:
            return 'Low';
    }
} 

//Shows Background colors options
function showBGColors(event) {
    const cardFormBackground = event.currentTarget.parentNode.parentNode;

    const bgColorsContainer = document.querySelector(".bg-color-container");
    bgColorsContainer.classList.toggle("bg-colors-container-show");

    const colorOptions = document.querySelectorAll(".color-option");

    //Adding appropriate BG color to each color-option and enabling them to close color pannel by clicking them
    colorOptions.forEach((option) => {
        option.style.backgroundColor = option.getAttribute('data-color');
        option.addEventListener('click', function () {
            let selectedColor = option.getAttribute('data-color');
            cardFormBackground.style.backgroundColor = selectedColor;
        });
    });
}

function handleShowEditCardModal(event){
    let targetCard = event.currentTarget;
    cardModal.classList.add("modal-show");

    let card = allCards.find(x => x.id === targetCard.getAttribute("data-id"));

    let isCompletedImg = card.completed === true ? "check.svg" : "circle.svg";
    let cardStatus = card.completed === true ? "Completed" : "In progress";

    let whenEdited = showEditedTime(card.editedDate);

    cardModal.innerHTML = `
        <div class="modal-card-form-container">
            <div class="edit-card-modal" data-id="${ card.id}" style="background-color: ${ card.backgroundColor }">
                <div class="edit-card-modal_header">
                    <textarea class="edit-input-title" data-id="${ card.id}"/>${ card.title }</textarea>
                    <image class="edit-card-title_img" src="./images/${ isCompletedImg}" alt="${cardStatus}" data-status="${ card.completed }"/>
                </div>
                <div class="edit-card-modal_body">
                    <textarea class="edit-input-text" data-id="${ card.id}">${ card.text }</textarea>
                    <div class="edit-date-container">
                        <p class="edit-date">${ whenEdited }</p>
                    </div>
                </div>
                <div class="edit-card-modal_footer">
                    <div class="priority-slider-container">
                        <div class="priority-label">Priority: ${ getPriorityText(card.priority)}</div>
                        <input type="range" min="1" max="3" value="${ card.priority}" class="priority-slider">
                    </div>
                    <button onclick="showBGColors(event)" type="button" class="new-card-form_color-button"><i class="fa-solid fa-palette"></i></button>
                    <button type="button" class="new-card-form_btn new-card-form_btn-delete">Delete</button>
                    <button onClick="closeModalWindow()" class="new-card-form_btn new-card-form_btn-close">Close</button>
                </div>
            </div>
            <div class="bg-color-container" data-id="${ card.id }">
                <div class="color-option" data-color="white"></div>
                <div class="color-option" data-color="#ffb5a7"></div>
                <div class="color-option" data-color="#d0f4de"></div>
                <div class="color-option" data-color="#bde0fe"></div>
            </div>
        </div>
    `;

    //Title
    const textTitleEl = document.querySelector('.edit-input-title');
    textTitleEl.style.height = 'auto';
    textTitleEl.style.height = textTitleEl.scrollHeight + 'px';
    textTitleEl.addEventListener('input', function (event) {
        resizeTextarea(event);
        handleEditCard(card);
    });

    //Status (completed / in progress)
    const cardStatusEl = document.querySelector('.edit-card-title_img');
    cardStatusEl.addEventListener('click', function () {
        cardStatusEl.src = cardStatusEl.src === `${rootAddress}/images/circle.svg` ? `${rootAddress}/images/check.svg` : `${rootAddress}/images/circle.svg`;
        cardStatusEl.alt = cardStatusEl.alt === "Completed" ? "In progress" : "Completed";
        let oldStatus = cardStatusEl.getAttribute("data-status") === "true";
        cardStatusEl.setAttribute("data-status", `${!oldStatus}`);
        handleEditCard(card);
    });

    //Tex body
    const textBodyEl = document.querySelector('.edit-input-text');
    textBodyEl.style.height = 'auto';
    textBodyEl.style.height = textBodyEl.scrollHeight + 'px';
    textBodyEl.addEventListener('input', function (event) {
        resizeTextarea(event);
        handleEditCard(card);
    });

    //Priority slider
    const sliderEl = document.querySelector('.priority-slider');
    const labelEl = document.querySelector('.priority-label');
    sliderEl.addEventListener('input', function () {
        let priority = parseInt(sliderEl.value);
        labelEl.textContent = 'Priority: ' + getPriorityText(priority);

        handleEditCard(card);
    });

    //Background color options
    const colorOptions = document.querySelectorAll(".color-option");
    function handleColorChange(event) {
        const modalBackground = document.querySelector(".edit-card-modal");
        modalBackground.style.backgroundColor = event.target.dataset.color;
        handleEditCard(card);
    };
    colorOptions.forEach(element => {
        element.addEventListener('click', (event) => handleColorChange(event));
    });

    //Delete button
    const deleteButton = document.querySelector(".new-card-form_btn-delete");
    deleteButton.addEventListener('click', () => handleDeleteCard(card));
    deleteButton.addEventListener('click', () => closeModalWindow());

    //Close modal window button
    const closeButton = document.querySelector(".new-card-form_btn-close");
    closeButton.addEventListener('click', () => closeModalWindow());
}

function handleEditCard(card) {
    const modalTitle = document.querySelector(".edit-input-title").value;
    const modalText = document.querySelector(".edit-input-text").value;
    const modalStatus = document.querySelector(".edit-card-title_img").getAttribute("data-status") === "true";
    const modalPriority = parseInt(document.querySelector(".priority-slider").value);
    const modalBackground = document.querySelector(".edit-card-modal").style.backgroundColor;

    const modalEditDateEl = document.querySelector(".edit-date");

    card.backgroundColor = modalBackground;
    card.title = modalTitle;
    card.text = modalText;
    card.priority = modalPriority;
    card.completed = modalStatus;
    card.editedDate = new Date();
    card.hasUnsavedChanges = true;

    modalEditDateEl.innerText = showEditedTime(card.editedDate);

    debouncedUpdateCards();

    showAllCards();
}

function showEditedTime(editedDate) {
    let result = "Edited: ";

    let now = new Date();
    let then = new Date(editedDate);
    let yearsDiff = now.getFullYear() - then.getFullYear();
    let mothDiff = now.getMonth() - then.getMonth();
    let daysDiff = now.getDate() - then.getDate();

    if ((daysDiff + mothDiff + yearsDiff) == 0) {
        result += then.getHours() + ":";
        result += then.getMinutes() < 10 ? `0` + `${then.getMinutes()}` : `${then.getMinutes()}`; // if minutes less than 10, then add 0-prefix before minutes (:07min) 
    }

    if (yearsDiff == 0 && (daysDiff != 0 || mothDiff != 0)) {
        result += then.getDate() + " " + then.toLocaleString('en-EN', { month: 'short' });
    }

    if (yearsDiff > 0) {
        result += then.getDate() + " " + then.toLocaleString('en-EN', { month: 'short' }) + ". " + then.getFullYear();
    }

    return result;
}

//Closing modal window by clicking "Cancel" button
const closeModalWindow = () => {
    cardModal.classList.remove("modal-show");
    cardModal.innerHTML = "";
};

//Closing modal window by tapping "Escape" buton on keyboard
window.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        closeModalWindow();
    }
});
//Modal window section start end



function sendNewCard(){
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
            showAllCards();

            alert("Congrats! Now you have a new task");
            closeModalWindow();
        })
        .catch(error => {
            alert(`${ error }. Try to add task again`);
        });
}

function changeSortDirection() {
    let upArrowEl = document.querySelector(".sort-direction_img-up");
    let downArrowEl = document.querySelector(".sort-direction_img-down");

    upArrowEl.classList.toggle("active-sort-arrow");
    downArrowEl.classList.toggle("active-sort-arrow");

    showAllCards();
}

function sortCards(collection) {
    let selector = document.getElementById("sort-select").value;

    let activeSortArrowEl = document.querySelector(".active-sort-arrow");
    let sortDirection = activeSortArrowEl.getAttribute("sort-direction");

    if (selector === "title") {
        return collection.sort(function (a, b) {
            if (a[selector].toLowerCase() < b[selector].toLowerCase()) {
                return sortDirection === "up" ? -1 : 1;
            }
            if (a[selector].toLowerCase() > b[selector].toLowerCase()) {
                return sortDirection === "up" ? 1 : -1;
            }
            return 0;
        });
    }

    return collection.sort(function (a, b) {
        if (a[selector] < b[selector]) {
            return sortDirection === "up" ? -1 : 1;
        }
        if (a[selector] > b[selector]) {
            return sortDirection === "up" ? 1 : -1;
        }
        return 0;
    });
}

function filterAllCards() {
    let inProgressCards = allCards.filter(x => x.completed === false);
    let doneCards = allCards.filter(x => x.completed === true);
    const showInProgress = checkboxInProgressEl.checked;
    const showDone = checkboxDoneEl.checked;

    let filteredCards = [];

    if (showInProgress) {
        filteredCards = filteredCards.concat(inProgressCards);
    }

    if (showDone) {
        filteredCards = filteredCards.concat(doneCards);
    }

    if (!showInProgress && !showDone) {
        return allCards;
    }

    return filteredCards;
}

function showAllCards() {
    mainContentEl.innerHTML = '';
    let currentCards = filterAllCards();
    currentCards = sortCards(currentCards);

    currentCards.forEach((card) => {
        let newCard = createNewCard(card);
        mainContentEl.appendChild(newCard);
    });
}


function createNewCard(card) {
    let isCompletedImg = card.completed === true ? "check.svg" : "circle.svg";
    let cardStatus = card.completed === true ? "Completed" : "In progress";

    let cardEl = document.createElement("div");
    cardEl.setAttribute("data-id", card.id);
    cardEl.classList.add("card");
    cardEl.innerHTML = `
        <div class="card-header">
            <p class="card-title">${ card.title }</p>
            <image class="card-title_img" onClick="toggleCardStatus(event)" src="./images/${ isCompletedImg }" alt="${ cardStatus }" data-id="${ card.id }"/>
        </div>
        <div class="card-body">
            <p class="card-text">${ card.text }</p>
        </div>
        <div class="card-footer">
            <p class="card-footer_priority">${ getPriorityText(card.priority) }</p>
        </div>
    `;

    cardEl.addEventListener("click", (event) => {
        handleShowEditCardModal(event);
    })
    cardEl.style.backgroundColor = card.backgroundColor;

    return cardEl;
}

function toggleCardStatus(event) {
    let cardImageEl = event.currentTarget;
    cardImageEl.src = cardImageEl.src === `${ rootAddress }/images/circle.svg` ? `${ rootAddress }/images/check.svg` : `${ rootAddress }/images/circle.svg`;
    cardImageEl.alt = cardImageEl.alt === "Completed" ? "In progress" : "Completed";

    let cardId = cardImageEl.getAttribute("data-id");
    let card = allCards.find(x => x.id === cardId);
    card.completed = !card.completed;
    card.editedDate = new Date();
    card.hasUnsavedChanges = true;

    debouncedUpdateCards();

    event.stopPropagation();
}

const debouncedUpdateCards = _.debounce(updateCards, 1500);

function updateCards() {
    let targetCards = allCards.filter(x => x.hasUnsavedChanges === true);

    let cardsToSend = targetCards.map(x => new Object({ ...x }));
    cardsToSend.forEach(x => x.editedDate = x.editedDate.toISOString().slice(0, 19)); // Making date without timestamp

    let requestOptions = {
        method: 'POST',
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify(cardsToSend)
    };

    fetch(`${rootAddress}/api/Task/update-cards`, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Something went wrong');
            }
            targetCards.forEach(x => x.hasUnsavedChanges = false);
            showAllCards();
        })
        .catch(error => {
            alert(`${error}. Try to edit later`);
            showAllCards();
        });
}

function handleDeleteCard(card) {
    let shouldDleteCard = confirm("Are You shure You want to delete this note ?");

    if (shouldDleteCard) {
        let indexToRemove = allCards.findIndex(x => x.id === card.id);
        let title = card.title;

        if (indexToRemove !== -1) {
            let promiseResult = deleteCard(card.id);
            promiseResult
                .then(data => {
                    allCards.splice(indexToRemove, 1);
                    showAllCards();
                    alert(`Successfully deleted ${title}`);
                })
                .catch(error => {
                    alert(`Cannot deleted note ${title}. ${error.message}`);
                })
        }
    }
}

function deleteCard(id) {
    let requestOptions = {
        method: 'DELETE',
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    };

    return fetch(`${ rootAddress }/api/Task/delete-card/?id=${ id }`, requestOptions)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Something went wrong');
                    }
                    return response.json();
                })
                .catch(error => {
                    throw error;
                });
}

function toggleCheckbox(event) {
    let checkbox = event.target;
    checkbox.checked = !checkbox.checked;
}