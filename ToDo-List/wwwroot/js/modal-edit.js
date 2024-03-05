
import { resizeTextarea, getPriorityText, showBGColors, showEditedTime, closeModalWindow, getLocaldateTimeString, rootAddress, refreshTokens } from './common.js';
import { allCards, showAllCards } from './site.js';

let fingerprint = "";

import('https://openfpcdn.io/fingerprintjs/v4')
    .then(FingerprintJS => FingerprintJS.load())
    .then(fp => fp.get())
    .then(result => fingerprint = result.visitorId);

function handleShowEditCardModal(event) {
    let targetCardEl = event.currentTarget;
    const cardModal = document.querySelector(".card-modal");
    cardModal.classList.add("modal-show");

    let card = allCards.find(x => x.id === targetCardEl.getAttribute("data-id"));

    let isCompletedImg = card.completed === true ? "check.svg" : "circle.svg";
    let cardStatus = card.completed === true ? "Completed" : "In progress";

    let whenEdited = showEditedTime(card.editedDate);

    cardModal.innerHTML = `
        <div class="modal-card-form-container">
            <div class="edit-card-modal" data-id="${card.id}" style="background-color: ${card.backgroundColor}">
                <div class="edit-card-modal_header">
                    <textarea class="edit-input-title" data-id="${card.id}"/>${card.title}</textarea>
                    <image class="edit-card-title_img" src="./images/${isCompletedImg}" alt="${cardStatus}" data-status="${card.completed}"/>
                </div>
                <div class="edit-card-modal_body">
                    <textarea class="edit-input-text" data-id="${card.id}">${card.text}</textarea>
                    <div class="edit-date-container">
                        <p class="edit-date">${whenEdited}</p>
                    </div>
                </div>
                <div class="edit-card-modal_footer">
                    <div class="priority-slider-container">
                        <div class="priority-label">Priority: ${getPriorityText(card.priority)}</div>
                        <input type="range" min="1" max="3" value="${card.priority}" class="priority-slider">
                    </div>
                    <button type="button" class="new-card-form_color-button"><i class="fa-solid fa-palette"></i></button>
                    <button type="button" class="new-card-form_btn new-card-form_btn-delete">Delete</button>
                    <button class="new-card-form_btn new-card-form_btn-close">Close</button>
                </div>
            </div>
            <div class="bg-color-container" data-id="${card.id}">
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
    const statusImgEl = document.querySelector('.edit-card-title_img');
    statusImgEl.addEventListener('click', function () {
        statusImgEl.src = statusImgEl.src === `${rootAddress}/images/circle.svg` ? `${rootAddress}/images/check.svg` : `${rootAddress}/images/circle.svg`;
        statusImgEl.alt = statusImgEl.alt === "Completed" ? "In progress" : "Completed";
        let oldStatus = statusImgEl.getAttribute("data-status") === "true";
        statusImgEl.setAttribute("data-status", `${ !oldStatus }`);
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


    //Show background color button
    const colorBtn = document.querySelector(".new-card-form_color-button");
    colorBtn.addEventListener('click', (event) => showBGColors(event));


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
    deleteButton.addEventListener('click', async () => await handleDeleteCard(card));
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

    showAllCards(allCards);
}

function toggleCardStatus(event) {
    let cardImageEl = event.currentTarget;
    cardImageEl.src = cardImageEl.src === `${rootAddress}/images/circle.svg` ? `${rootAddress}/images/check.svg` : `${rootAddress}/images/circle.svg`;
    cardImageEl.alt = cardImageEl.alt === "Completed" ? "In progress" : "Completed";

    let cardId = cardImageEl.getAttribute("data-id");
    let card = allCards.find(x => x.id === cardId);
    card.completed = !card.completed;
    card.editedDate = new Date();
    card.hasUnsavedChanges = true;

    debouncedUpdateCards();
}

const updateCardsAsyncWrapper = async () => {
    await updateCards(false);
};

const debouncedUpdateCards = _.debounce(updateCardsAsyncWrapper, 1000);

// Parameter isSecondCall is made for preventing endless recursive loop
async function updateCards(isSecondCall) {
    let targetCards = allCards.filter(x => x.hasUnsavedChanges === true);

    let cardsToSend = targetCards.map(x => new Object({ ...x }));
    cardsToSend.forEach(x => x.editedDate = getLocaldateTimeString(x.editedDate)); // Making date without timestamp

    let requestOptions = {
        method: 'POST',
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify(cardsToSend)
    };

    try {
        let response = await fetch(`${rootAddress}/api/Task/update-cards`, requestOptions);
        if (response.status === 401) {
            if (isSecondCall === true) {
                throw new Error('Your current session has beeen expired');
            }

            let tokensAreRefreshed = await refreshTokens(fingerprint);

            if (tokensAreRefreshed) {
                await updateCards(true);
                return true;
            } else {
                throw new Error('Your current session has beeen expired');
            }
        }

        if (response.status !== 200) {
            throw new Error('Something went wrong');
        }

        targetCards.forEach(x => x.hasUnsavedChanges = false);
        showAllCards(allCards);
    }
    catch (error) {
        alert(`${error}. Try to edit later`);
        window.location.assign(rootAddress);
    }
}

async function handleDeleteCard(card) {
    let shouldDleteCard = confirm("Are You shure You want to delete this note ?");

    if (shouldDleteCard) {
        let indexToRemove = allCards.findIndex(x => x.id === card.id);

        if (indexToRemove !== -1) {
            let isDeleted = await deleteCard(card.id, false);
            if (isDeleted) {
                allCards.splice(indexToRemove, 1);
                showAllCards(allCards);
                alert(`Successfully deleted ${card.title}`);
            } else {
                alert(`Cannot deleted note ${card.title}. ${error.message}`);
            }
        }
    }
}

// Parameter isSecondCall is made for preventing endless recursive loop
async function deleteCard(id, isSecondCall) {
    let requestOptions = {
        method: 'DELETE',
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    };

    try {
        let response = await fetch(`${rootAddress}/api/Task/delete-card/?id=${id}`, requestOptions);

        if (response.status === 401) {
            if (isSecondCall === true) {
                return false;
            }

            let tokensAreRefreshed = await refreshTokens(fingerprint);

            if (tokensAreRefreshed) {
                return await deleteCard(id, true);
            } else {
                alert('Your current session has beeen expired');
                window.location.assign(rootAddress);
            }
        }

        if (response.status !== 200) {
            throw new Error('Something went wrong');
        }

        return true;
    }
    catch (error) {
        console.log(error);
        return false;
    }
}


export { handleShowEditCardModal, handleEditCard, toggleCardStatus };