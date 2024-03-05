'use strict';

import { handleShowEditCardModal, toggleCardStatus } from './modal-edit.js';
import { handleShowAddNewCardModal } from './modal-add.js';
import { rootAddress, getPriorityText, closeModalWindow } from './common.js';

let allCardsEl = document.getElementById("allCards");
let allCards = JSON.parse(allCardsEl.value);
allCards.forEach(x => x.editedDate = new Date(x.editedDate));
showAllCards(allCards);


const sortSelectorEl = document.getElementById("sort-select");
sortSelectorEl.addEventListener('change', () => showAllCards(allCards));

const sortDirectionEl = document.querySelector(".sort-direction");
sortDirectionEl.addEventListener('click', () => changeSortDirection(allCards));


const searchContainerEl = document.querySelector(".search-container");
const inputSearchEl = document.querySelector(".search-input");
const searchTextRemoveBtnEl = document.querySelector(".search-text-remove-btn");
inputSearchEl.addEventListener('focus', function () {
    searchContainerEl.classList.add('search-container--active');
    searchTextRemoveBtnEl.classList.add('search-text-remove-btn--visible');
});

inputSearchEl.addEventListener('blur', function () {
    if (!inputSearchEl.value) {
        searchContainerEl.classList.remove('search-container--active');
        searchTextRemoveBtnEl.classList.remove('search-text-remove-btn--visible');
    }

    inputSearchEl.style.minWidth = "280px";
});

inputSearchEl.addEventListener('input', function () {
    let requestedText = inputSearchEl.value.toLowerCase();
    searchCard(requestedText, allCards);
});

searchTextRemoveBtnEl.addEventListener('click', function () {
    inputSearchEl.value = '';
    showAllCards(allCards);
    searchContainerEl.classList.remove('search-container--active');
    searchTextRemoveBtnEl.classList.remove('search-text-remove-btn--visible');
})

const toggleFilterElements = document.querySelectorAll(".toggle");
toggleFilterElements.forEach(element => {
    element.addEventListener('click', () => showAllCards(allCards))
});


const addBtn = document.querySelector(".add-button");
addBtn.addEventListener('click', () => handleShowAddNewCardModal());


const logoutBtn = document.querySelector(".logout-button");
logoutBtn.addEventListener('click', () => handleLogout());

//Closing modal window by tapping "Escape" buton on keyboard
window.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        closeModalWindow();
    }
});


function searchCard(text, cardsCollection) {
    let cards = cardsCollection.filter(x => x.title.toLowerCase().includes(text) || x.text.toLowerCase().includes(text));
    showAllCards(cards);
}

function changeSortDirection() {
    let upArrowEl = document.querySelector(".sort-direction_img-up");
    let downArrowEl = document.querySelector(".sort-direction_img-down");

    upArrowEl.classList.toggle("active-sort-arrow");
    downArrowEl.classList.toggle("active-sort-arrow");

    showAllCards(allCards);
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

function filterAllCards(cards) {
    let inProgressCards = cards.filter(x => x.completed === false);
    let doneCards = cards.filter(x => x.completed === true);

    const checkboxInProgressEl = document.querySelector(".toggle-checkbox-in-progress");
    const showInProgress = checkboxInProgressEl.checked;

    const checkboxDoneEl = document.querySelector(".toggle-checkbox-done");
    const showDone = checkboxDoneEl.checked;

    let filteredCards = [];

    if (showInProgress) {
        filteredCards = filteredCards.concat(inProgressCards);
    }

    if (showDone) {
        filteredCards = filteredCards.concat(doneCards);
    }

    if (!showInProgress && !showDone) {
        return cards;
    }

    return filteredCards;
}

function showAllCards(cardsCollection) {
    const mainContentEl = document.querySelector(".main-content");

    mainContentEl.innerHTML = '';
    let currentCards = filterAllCards(cardsCollection);
    currentCards = sortCards(currentCards);

    currentCards.forEach((card) => {
        let newCard = createNewCard(card);
        mainContentEl.appendChild(newCard);
    });

    //Status (completed / in progress)
    const statusImgElements = document.querySelectorAll(".card-title_img");
    statusImgElements.forEach(statusImgEl => {
        statusImgEl.addEventListener('click', function (event) {
            toggleCardStatus(event);
            event.stopPropagation();
        })
    })
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
            <image class="card-title_img" src="./images/${ isCompletedImg}" alt="${cardStatus}" data-status="${card.completed}" data-id="${card.id}"/>
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

function handleLogout() {
    let shouldLogout = confirm("Are you shure you want to logout?");

    if(shouldLogout) {
        fetch(rootAddress + "/api/auth/logout", {
            method: 'GET',
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Eror! Try again later`);
                }

                window.location.assign(rootAddress);
            })
            .catch(error => {
                alert(error)
            });
    }
}



export { allCards, showAllCards };