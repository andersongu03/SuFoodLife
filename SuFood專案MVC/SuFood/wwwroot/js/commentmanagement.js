let app = new Vue({
    el: ".CommentReview",
    data:
    {
        keyword:"",
        topic: '評論管理',
        kesi: "",
        deleteId: undefined,
        reviewId: undefined,
        OrdersId: undefined,
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
        createComment() {
            /*console.log('123')*/
            this.CreateOrEditOrDelete = 'Create';
            this.popupShowing.showPopup= true;
           
        },
        CloseComment() {
            /*console.log('123')*/
            this.popupShowing.showPopup = false;
        },
        closepopupShowHint() {
            this.popupShowing.showPopup = false;
            this.toastHint();
        },
        editComment(item) {
            /*alert('213')*/
            /*console.log(item);*/
            this.CreateOrEditOrDelete = 'Edit';
            this.popupShowing.showPopup = true; 
            this.editCommentList = item;
            
            //this.reviewId=item.reviewId
        },
        deleteComment(c) {
            /*console.log(c);*/
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
            /*console.log("觸發");*/
            var _this = this
            axios.delete(`/BackStage/CommentManagement/DeleteComment/${id}`).then(response => {
                this.kesi = response.data;
                this.closepopupShowHint()
                _this.GetComments();
            })
        },
        CreateComment(createCommentList) {
            let _this = this;

            var request = null;

            request = {
                    "reviewId": _this.createCommentList.reviewId,
                    "ratingStar": _this.createCommentList.ratingStar,
                    "comment": _this.createCommentList.comment,
                    "ordersId": _this.createCommentList.ordersId
             }


            if (this.createCommentList.ordersId == 0 || this.createCommentList.ratingStar > 6 || this.createCommentList.ratingStar < 0) {
                alert('客戶ID為必填欄位,星數不得大於5或是小於0')
            }
            else {
                axios.post('/BackStage/CommentManagement/CreateComment', request 
                ).then(response => {
                    _this.kesi = response.data
                    /*console.log('123')*/
                    this.closepopupShowHint()
                    _this.GetComments()
                })
            }
        },
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
                this.kesi = response.data,
                this.closepopupShowHint()
                _this.GetComments()
            })
        }

    },
    computed: {
        filter() {
            //篩選
            console.log('123')
            return this.or.filter(ri => ri.reviewId.toLowerCase().includes(this.keyword.toLowerCase()))
            arr = this.or.filter(o => {
                return o.or.indexOf(this.keyword)!=-1
            })
        }
    },
    mounted() {
        this.GetComments();
    }
})