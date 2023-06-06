let app = new Vue({
    el: ".CommentReview",
    data:
    {
        keyword:"",
        topic: '評論管理',
        toast: "",
        deleteId: undefined,
        reviewId: undefined,
        ordersId: 0,
        popupShowing: {
            showPopup: false
        },
        toastHintStyle: {
            fadeInUp: false
        },
        CreateOrEditOrDelete: "",
        or: [],
        createCommentList: {
            reviewId: 0,
            ratingStar:0,
            comment:"",
            ordersId:0
        },
        editCommentList: {
            ReviewId: "",
            Comment: '',
            OrdersId: '',
        },
        modalContentStyle: {
            w200: true,
            w800: false
        },
        modalContainerStyle: {
            showModal: false,
        },
        isActive: false,
        selectedStars: 'all',
    },
    request: {
        reviewId: "",
        ratingStar:"",
        ordersId: "",
        comment: "",
    },
    methods: {
        toastHint() {
            this.toastHintStyle.fadeInUp = true;
            setTimeout(() => {
            this.toastHintStyle.fadeInUp = false
            }, 2000)
        },
        createComment(id) {
            this.OrdersId = id;
            this.CreateOrEditOrDelete = 'Create';
            this.popupShowing.showPopup = true;
        },
        CloseComment() {
            this.popupShowing.showPopup = false;
        },
        closepopupShowHint() {
            this.popupShowing.showPopup = false;
            this.toastHint();
        },
        editComment(item) {
            this.CreateOrEditOrDelete = 'Edit';
            this.popupShowing.showPopup = true; 
            this.editCommentList = item;
        },
        deleteComment(c) {
            this.CreateOrEditOrDelete = 'Delete';
            this.popupShowing.showPopup = true;
            this.editCommentList = c;
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
            var _this = this
            axios.delete(`/BackStage/CommentManagement/DeleteComment/${id}`).then(response => {
                this.toast = response.data;
                this.closepopupShowHint()
                _this.GetComments();
            })
        },
        CreateComment(or) {
            let _this = this;
            var request = null;
            createCommentList = {
                reviewId: _this.createCommentList.reviewId,
                ratingStar: _this.createCommentList.ratingStar,
                comment: _this.createCommentList.comment,
                ordersId: _this.ordersId
            }

            axios.post('https://localhost:7086/MyOrders/CreateComment/', createCommentList).then(response => {
                this.toast = response.data;
                this.closepopupShowHint();
                this.GetDetail();
            })
        },
        //沒用到
        EditComment: function (editCommentList) {
            var _this = this;
            var Cdata = {
                reviewId: _this.editCommentList.reviewId,
                ratingStar: _this.editCommentList.ratingStar,
                comment: _this.editCommentList.comment,
                ordersId: _this.editCommentList.ordersId
            };
            axios.post('/BackStage/CommentManagement/EditComment/', Cdata,{
                headers: {
                    'Content-Type':'application/json'
                }
            }).then(response => {
                this.toast = response.data,
                this.closepopupShowHint()
                _this.GetComments()
            })
        }
    },
    computed: {
        filterproducts() {
            let arr = this.or
            if (this.keyword != '') {
                arr = arr.filter(o => { return o.ordersId == this.keyword })
            }

            //下拉式選單篩選結果

            
            switch (this.selectedStars) {
                case "all":
                    break;
                case "good":
                    arr = arr.filter(o => {
                        return o.ratingStar == 5
                    })
                    break;
                case "bad":
                    arr = arr.filter(o => {
                        return o.ratingStar >= 1 && o.ratingStar < 5
                    })
                    break;
            }

            return arr
        }
    },
    mounted() {
        this.GetComments();
    }
})