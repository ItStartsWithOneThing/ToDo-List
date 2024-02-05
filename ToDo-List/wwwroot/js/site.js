
//Modal window section start
const addNewCardModal = document.querySelector(".add-new-card-modal");

const handleShowAddNewCardInput = () => {
    addNewCardModal.classList.add("modal-show");
    document.body.classList.add("stop-scrolling");

    addNewCardModal.innerHTML = `
	<div class="new-card-form-container">
        <div class="new-card-form_background">
            <form id="new-card-form-id" class="new-card-form">
               <input class="input-title" placeholder="Enter card name"/>
               <textarea class="input-text" placeholder="Enter text"></textarea>
            </form>
            <div class="new-card-form-navigation">
                <div class="priority-slider-container">
                    <div class="priority-label">Priority: Low</div>
                    <input type="range" min="1" max="3" value="1" class="priority-slider">
                </div>
                <button onclick="" type="button" class="new-card-form_color-button"><i class="fa-solid fa-palette"></i></button>
                <button type="button" onClick="sendNewCard()" class="new-card-form_btn">Confirm</button>
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

    const slider = document.querySelector('.priority-slider');
    const label = document.querySelector('.priority-label');

    slider.addEventListener('input', function () {
        let position = parseInt(slider.value);

        switch (position) {
            case 1:
                label.textContent = 'Priority: Low';
                break;
            case 2:
                label.textContent = 'Priority: Medium';
                break;
            case 3:
                label.textContent = 'Priority: High';
                break;
            default:
                label.textContent = 'Priority: Low';
        }
    });

    const colorButton = document.querySelector(".new-card-form_color-button");
    colorButton.addEventListener("click", () => showBGColors());

    const btnClose = document.querySelector(".new-card-form_btn-close");
    btnClose.addEventListener("click", () => closeModalWindow());
};

//Shows Background colors options
const showBGColors = () => {
    const newCardFormBackground = document.querySelector(".new-card-form_background");

    const bgColorsContainer = document.querySelector(".bg-color-container");
    bgColorsContainer.classList.toggle("bg-colors-container-show");

    const colorOptions = document.querySelectorAll(".color-option");

    //Adding appropriate BG color to each color-option and enabling them to close color pannel by clicking them
    colorOptions.forEach((option) => {
        option.style.backgroundColor = option.getAttribute('data-color');
        option.addEventListener('click', function () {
            let selectedColor = option.getAttribute('data-color');
            newCardFormBackground.style.backgroundColor = selectedColor;

            console.log(newCardFormBackground);
        });
    });
}

//Closing modal window by clicking "Cancel" button
const closeModalWindow = () => {
    addNewCardModal.classList.remove("modal-show");
    document.body.classList.remove("stop-scrolling");
};

//Closing modal window by tapping "Escape" buton on keyboard
window.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        closeModalWindow();
    }
});
//Modal window section start end


const filterCardsBy = () => {

}

let allCards = JSON.parse(document.getElementById("allCards").value);

const sendNewCard = async () => {
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

    const response = await fetch("https://localhost:7271/api/Task/add-card", requestOptions);

    if (await response.ok) {
        const responseCard = await response.json();

        allCards.add(JSON.parse(responseCard));
    }
    else {
        alert("Try to add task again");
    }

    closeModalWindow();
}


const mainContent = document.querySelector(".main-content");

allCards.forEach((card) => {
    let isCompletedImg = card.completed === true ? `check.svg` : `circle.svg`;
    let cardStatus = card.completed === true ? "Completed" : "In progress";
    const cardEl = document.createElement("div");
    cardEl.classList.add("card");
    cardEl.innerHTML = `
    <div class="card-header">
      <p class="card-title">${card.title}</p>
      <image class="card-title_img" src="./images/${isCompletedImg}" alt="${cardStatus}" />
    </div>
    <div class="card-body">
      <p class="card-text">${card.Text}</p>
    </div>
    <div class="card-footer">
      <p class="card-footer_priority">${card.priority}</p>
    </div>
    <input type="hidden" id="cardId" value="${card.id}"  />
    `;

    mainContent.appendChild(cardEl);
});



 