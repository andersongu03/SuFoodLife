let app = new Vue({
    el: ".CommentReview",
    data:
    {   
        topic: '評論管理',
        kesi: "",
        deleteId: undefined,
        ReviewId: undefined,
        OrdersId:undefined,
        popupShowing:{
            showPopup: false
        },
        toastHintStyle: {
            fadeInUp: false
        },
        CreateOrEditOrDelete:"",
        or: [],
        editCommentList:{
            ReviewId: this.ReviewId,
            Comment: '',
            OrdersId: this.OrdersId
        },
        modalContentStyle: {
            w200: true,
            w800: false
        },
        modalContainerStyle: {
            showModal: false,
        },
        isActive: false,
    },
    request: {
        ordersId: 0,
        comment: "",
    },
    methods: {
        toastHint() {
            this.toastHintStyle.fadeInUp = true;
            setTimeout(() => {
            this.toastHintStyle.fadeInUp = false
            }, 2000)
        },
        createComment() {
            this.CreateOrEditOrDelete = 'Create';
            this.popupShowing = true;
        },
        CloseComment() {
            this.popupShowing.showPopup = false;
        },
        ClosepopupShowHint() {
            this.popupShowing.showPopup = false;
            this.toastHint();
        },
        editComment(item) {
            this.CreateOrEditOrDelete = 'Edit';
            this.popupShowing.showPopup = true; 
            this.editCommentList = item,
            this.ReviewId=item.reviewId
        },
        DeleteComment(c) {
            this.CreateOrEditOrDelete = 'Delete';
            this.popupShowing.showPopup = true;
            this.deleteId = c.ordersId
            console.log(this.deleteId)
        },
        openModal() {
            this.modalContainerStyle.showModal = true;
        },
        closeModal() {
            this.modalContainerStyle.showModal = false;
        },
        GetComments() {
            axios.get("/BackStage/CommentManagement/GetComment").then(response => {
                this.or = response.data
            })
        },
        DeleteComment: function (id) {
            /*console.log("觸發");*/
            var _this = this
            axios.delete(`/BackStage/CommentManagement/DeleteComment/${id}`).then(response => {
                this.kesi = response.data;
                _this.GetComments();
            })
        },
        CreateComment() {
            let _this = this;
            axios.post('/Comment/CreateComment', this.request, {
                header: {
                    'Content-Type':'application/json'
                }
            }).then(response => {
                this.kesi = response.data
                this.closePopupShowHint()
                _this.GetComments()
            })
        },

    },
    mounted() {
        this.GetComments();
    }
})